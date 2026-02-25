
namespace PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
    public class AddEditPatiendCommandHandler : IRequestHandler<AddEditPatientWithUserId, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public AddEditPatiendCommandHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(AddEditPatientWithUserId request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.AddEditPatient(request);
            return patient;
        }
    }

