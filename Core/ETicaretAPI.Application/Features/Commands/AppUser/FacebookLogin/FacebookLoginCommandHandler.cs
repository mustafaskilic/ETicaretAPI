using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;

        public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClient.CreateClient();
            _configuration = configuration;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            string facebookDomain = "https://graph.facebook.com";
            string test = _configuration["ExternalLogins:Facebook:ClientId"];
            string accessTokenResponse = await _httpClient.GetStringAsync($"{facebookDomain}/oauth/access_token?client_id={_configuration["ExternalLogins:Facebook:ClientId"]}&client_secret={_configuration["ExternalLogins:Facebook:ClientSecret"]}&grant_type=client_credentials");
            FacebookAccessTokenResponse facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"{facebookDomain}/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponse.AccessToken}", cancellationToken);

            FacebookUserAccessTokenValidation validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation.Data.IsValid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"{facebookDomain}/me?fields=email,name&access_token={request.AuthToken}");

                FacebookUserInfoResponse userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                bool result = user != null;

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(userInfo.Email);
                    if (user is null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo.Email,
                            UserName = userInfo.Email,
                            NameSurname = userInfo.Name,
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }

                if (result)
                {
                    await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

                    Token token = _tokenHandler.CreateAccessToken(5);

                    return new()
                    {
                        Token = token
                    };
                }
            }

            throw new Exception("Invalid external authentication.");
        }
    }
}
