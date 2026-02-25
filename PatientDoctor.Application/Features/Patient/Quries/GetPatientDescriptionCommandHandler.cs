
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientDescriptionCommandHandler : IRequestHandler<GetPatientDescription, IResponse>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatientDescriptionCommandHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public Task<IResponse> Handle(GetPatientDescription request, CancellationToken cancellationToken)
        {
            return _patientRepository.GetPatientDescriptionById(request);
        }
    }

