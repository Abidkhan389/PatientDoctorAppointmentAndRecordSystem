using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserList : TableParam, IRequest<IResponse>
    {
        public string UserName { get; set; }
        public int? Status { get; set; }
        public string? Email { get; set; }
        public string? Cnic { get; set; }
        public string? MobileNumber { get; set; }
        public string? City { get; set; }

    }
}
