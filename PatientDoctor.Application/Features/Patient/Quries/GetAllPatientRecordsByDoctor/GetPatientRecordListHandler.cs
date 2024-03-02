using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor
{
    public class GetPatientRecordListHandler : IRequestHandler<GetPatientRecordListWithDoctor, IResponse>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatientRecordListHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(GetPatientRecordListWithDoctor request, CancellationToken cancellationToken)
        {
            return await _patientRepository.GetPatientsRecordWithDoctorProc(request);
        }
    }
}
