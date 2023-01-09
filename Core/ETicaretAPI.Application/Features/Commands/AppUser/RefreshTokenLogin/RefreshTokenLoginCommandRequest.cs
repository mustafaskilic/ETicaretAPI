using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandRequest : IRequest<RefreshTokenLoginCommandResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenLoginCommandResponse
    {
        public Token Token{ get; set; }
    }

    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        readonly IAuthService _authService;

        public RefreshTokenLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        async Task<RefreshTokenLoginCommandResponse> IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>.Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
            return new RefreshTokenLoginCommandResponse
            {
                Token = token
            };
        }
    }
}
