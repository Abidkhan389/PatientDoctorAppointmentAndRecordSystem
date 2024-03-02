using FluentValidation;

namespace PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf
{
    public class GetPatientDetailsForPdfValidation : AbstractValidator<GetPatientDetailsForPdfRequest>
    {
        public GetPatientDetailsForPdfValidation()
        {
            RuleFor(request => request.PatientId)
                .NotEmpty().WithMessage("PatientId is required")
                .Must(id => !string.IsNullOrWhiteSpace(id.ToString()))
                .WithMessage("PatientId must not be empty or contain whitespace");
            RuleFor(request => request.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required")
               .Must(doctorid => !string.IsNullOrWhiteSpace(doctorid))
               .NotEmpty().WithMessage("DoctorId must not be empty or contain whitespace");
        }
    }
}
