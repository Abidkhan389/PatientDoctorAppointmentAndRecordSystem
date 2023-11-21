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
    public class GetPatientAppoitmentsListWithDoctorHandler : IRequestHandler<GetPatientAppoitmentListWithDocter, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientAppoitmentsListWithDoctorHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public Task<IResponse> Handle(GetPatientAppoitmentListWithDocter request, CancellationToken cancellationToken)
        {
            return _patientRepository.GetAllPatientAppoitmentWithDoctorProc(request);
        }
    }
}
