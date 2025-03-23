using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class CurrentWeekPatientWithFeeCount :IRequest<IResponse>
    {
        public string logInUserId { get; set; }
        public string logInUserRole { get; set; }
    }
}
