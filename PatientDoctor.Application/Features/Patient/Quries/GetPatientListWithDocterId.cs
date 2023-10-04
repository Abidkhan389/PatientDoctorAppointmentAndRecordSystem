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
    public class GetPatientListWithDocterId: TableParam, IRequest<IResponse>
    {
        public GetPatientList  GetPatientListObj { get; }
        public string DocterId { get; }

        public GetPatientListWithDocterId(GetPatientList model, Guid docterId)
        {
            GetPatientListObj = model;
            DocterId = docterId.ToString();
        }
    }

}
