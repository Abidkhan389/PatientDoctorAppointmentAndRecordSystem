
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientListQueryHandler : IRequestHandler<GetPatientListWithUser, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientListQueryHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(GetPatientListWithUser request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetAllByProc(request);
            return patient;
        }
    }

