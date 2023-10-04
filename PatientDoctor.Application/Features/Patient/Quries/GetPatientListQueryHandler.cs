using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class GetPatientListQueryHandler : IRequestHandler<GetPatientListWithDocterId, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientListQueryHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }
        public async Task<IResponse> Handle(GetPatientListWithDocterId request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetAllByProc(request);
            return patient;
        }
    }
}
