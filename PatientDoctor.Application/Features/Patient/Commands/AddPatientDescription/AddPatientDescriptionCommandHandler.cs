
namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
    public class AddPatientDescriptionCommandHandler : IRequestHandler<AddPatientDescriptionCommand, IResponse>
    {
        private readonly IPatientRepository _patientRepository;
        public AddPatientDescriptionCommandHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public Task<IResponse> Handle(AddPatientDescriptionCommand request, CancellationToken cancellationToken)
        {
            return _patientRepository.AddEditPatientDescription(request);
        }
    }

