using FluentValidation;

namespace PatientDoctor.Application.Features.Doctor_Availability.Commands;
public class AddEditDoctorAvailabilityValidation : AbstractValidator<AddEditDoctorAvailabilityCommands>
{
    public AddEditDoctorAvailabilityValidation()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Doctor ID is required.");

        RuleFor(x => x.DayIds)
            .Must(day => day != null && day.Count > 0)
            .WithMessage("At least one Day time slot must be provided");

        RuleFor(x => x.DoctorTimeSlots)
            .NotNull().WithMessage("Doctor time slots are required.")
            .Must(slots => slots != null && slots.Count > 0)
            .WithMessage("At least one doctor time slot must be provided.");

        RuleForEach(x => x.DoctorTimeSlots).SetValidator(new DoctorTimeSlotValidator());
    }
}

public class DoctorTimeSlotValidator : AbstractValidator<DoctorTimeSlot>
{
    public DoctorTimeSlotValidator()
    {
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.")
            .Matches(@"^(0[1-9]|1[0-2]):[0-5][0-9] [APap][Mm]$")
            .WithMessage("Start time must be in the format 'hh:mm AM/PM'.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .Matches(@"^(0[1-9]|1[0-2]):[0-5][0-9] [APap][Mm]$")
            .WithMessage("End time must be in the format 'hh:mm AM/PM'.");

        RuleFor(x => x)
            .Must(slot => IsValidTimeRange(slot.StartTime, slot.EndTime))
            .WithMessage("End time must be after start time.");
    }

    private bool IsValidTimeRange(string startTime, string endTime)
    {
        if (DateTime.TryParse(startTime, out DateTime start) && DateTime.TryParse(endTime, out DateTime end))
        {
            return end > start;
        }
        return false;
    }
}