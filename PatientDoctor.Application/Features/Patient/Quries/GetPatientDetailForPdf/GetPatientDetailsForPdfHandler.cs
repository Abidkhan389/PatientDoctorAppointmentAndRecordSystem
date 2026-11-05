
namespace PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf;
    public class GetPatientDetailsForPdfHandler : IRequestHandler<GetPatientDetailsForPdfRequest, IResponse>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatientDetailsForPdfHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(GetPatientDetailsForPdfRequest request, CancellationToken cancellationToken)
        {
            return await _patientRepository.GetPatientDetailsForPdf(request);
        }
    }

