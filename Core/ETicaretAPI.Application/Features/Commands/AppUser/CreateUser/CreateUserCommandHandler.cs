﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUserModel = ETicaretAPI.Domain.Entities.Identity.AppUser;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<AppUserModel> _userManager;

        public CreateUserCommandHandler(UserManager<AppUserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                NameSurname = request.NameSurname
            }, request.Password);

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";

            else
                foreach (var error in result.Errors)
                    response.Message += $"${error.Code} - {error.Description}";

            return response;
            //throw new UserCreateFailedException();
        }
    }
}