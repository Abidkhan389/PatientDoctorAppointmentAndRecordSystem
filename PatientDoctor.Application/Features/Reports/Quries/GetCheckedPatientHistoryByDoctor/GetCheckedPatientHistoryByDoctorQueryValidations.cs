using FluentValidation;

namespace PatientDoctor.Application.Features.Reports.Quries.GetCheckedPatientHistoryByDoctor;
public class GetCheckedPatientHistoryByDoctorQueryValidations : AbstractValidator<GetCheckedPatientHistoryByDoctorQuery>
{
    public GetCheckedPatientHistoryByDoctorQueryValidations()
    {
        RuleFor(x => x.FromDate)
             .NotEmpty().WithMessage("From date is required.")
             .LessThanOrEqualTo(DateTime.Today)
             .WithMessage("From date cannot be in the future.");

        RuleFor(x => x.ToDate)
            .NotEmpty().WithMessage("To date is required.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("To date cannot be in the future.");
        // FromDate must be <= ToDate
        RuleFor(x => x)
            .Must(x => x.FromDate <= x.ToDate)
            .WithMessage("From date must be less than or equal to To date.");
        // Max range = 2 years
        RuleFor(x => x)
            .Must(x => (x.ToDate - x.FromDate).TotalDays <= 1097)
            .WithMessage("Date range cannot exceed 3 years.");
    }
}

