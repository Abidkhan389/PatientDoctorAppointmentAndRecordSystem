using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Administrator.Commands.Register;
    public class UserRegisterCommand : IRequest<IResponse>
    {
        public string Uname { get; set; }
        public string cpassword { get; set; }
        public string password { get; set; }
        
    }

