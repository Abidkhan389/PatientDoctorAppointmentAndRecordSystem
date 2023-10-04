using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<IResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
