using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class GetPatientAppoitmentListWithDocter : TableParam, IRequest<IResponse>
    {
        public GetPatientAppoitmentsList GetPatientAppoitmentsListObj { get; }
        public string DocterId { get; }
        public GetPatientAppoitmentListWithDocter(GetPatientAppoitmentsList model, Guid docterId)
        {
            GetPatientAppoitmentsListObj = model;
            DocterId = docterId.ToString();
        }
    }
}
