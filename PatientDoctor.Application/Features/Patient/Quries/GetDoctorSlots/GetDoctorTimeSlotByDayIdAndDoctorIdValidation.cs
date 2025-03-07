

using FluentValidation;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;

namespace PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
public class GetDoctorTimeSlotByDayIdAndDoctorIdValidation : AbstractValidator<GetDoctorTimeSlotsByDayIdAndDoctorId>
{
    public GetDoctorTimeSlotByDayIdAndDoctorIdValidation()
    {
        // DayId must be greater than 0
        RuleFor(x => x.DayId)
            .GreaterThan(0)
            .WithMessage("DayId must be greater than 0.");

        // DoctorId must not be empty (Valid GUID)
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .WithMessage("DoctorId is required.");
    }
}


