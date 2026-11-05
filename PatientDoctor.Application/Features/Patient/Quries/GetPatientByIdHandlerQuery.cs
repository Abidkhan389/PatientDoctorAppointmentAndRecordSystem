
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientByIdHandlerQuery : IRequestHandler<GetPatientById, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientByIdHandlerQuery(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(GetPatientById request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetPatientById(request);
            return patient;
        }
    }

