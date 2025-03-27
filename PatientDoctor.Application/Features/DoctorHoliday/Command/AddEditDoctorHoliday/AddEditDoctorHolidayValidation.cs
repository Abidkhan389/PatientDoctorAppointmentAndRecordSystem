
using FluentValidation;

namespace PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
public class AddEditDoctorHolidayValidation : AbstractValidator<AddEditDoctorHolidayCommand>
{
    public AddEditDoctorHolidayValidation()
    {
        // DoctorHolidayId: Must be a valid GUID if provided
        RuleFor(x => x.DoctorHolidayId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("DoctorHolidayId must be a valid GUID if provided.");

        // DoctorId: Required & greater than 0
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .WithMessage("DoctorId is required");
        // FromDate: Required
        RuleFor(x => x.FromDate)
            .NotNull()
            .WithMessage("FromDate is required.");

        // ToDate: If provided, must be greater than or equal to FromDate
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .WithMessage("ToDate must be on or after FromDate.");

        // DayOfWeek: Should be between 0 (Sunday) and 6 (Saturday)
        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("DayOfWeek must be a valid value between 0 (Sunday) and 6 (Saturday).");

        // Reason: Should not be only whitespace if provided
        RuleFor(x => x.Reason)
            .Must(reason => string.IsNullOrWhiteSpace(reason) || !string.IsNullOrWhiteSpace(reason.Trim()))
            .WithMessage("Reason should not be only whitespace if provided.");
    }
}

