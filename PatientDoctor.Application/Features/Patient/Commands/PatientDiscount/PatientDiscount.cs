using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.PatientDiscount
{
    public class PatientDiscount: IRequest<IResponse>
    {
        public Guid PatientId { get; set; }
        // public Guid PatientIdGuid { get; set; }
        public string DoctorId { get; set; }
        public int DiscountFee { get; set; }
    }
}
