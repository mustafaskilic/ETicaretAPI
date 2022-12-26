using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandRequest : IRequest<GoogleLoginCommandResponse>
    {
        public string ID { get; set; }
        public string IDToken { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl{ get; set; }
        public string Provider{ get; set; }
    }
}
