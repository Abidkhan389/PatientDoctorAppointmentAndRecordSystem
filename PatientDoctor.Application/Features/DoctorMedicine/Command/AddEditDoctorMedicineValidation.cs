
using FluentValidation;

namespace PatientDoctor.Application.Features.DoctorMedicine.Command;
public class AddEditDoctorMedicineValidation : AbstractValidator<AddEditDoctorMedicineCommand>
{
    public AddEditDoctorMedicineValidation()
    {
        RuleFor(x => x.MedicineId)
            .NotEmpty().WithMessage("Medicine Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Medicine Id must be a valid GUID.");

        RuleFor(x => x.DoctorIds)
            .NotEmpty().WithMessage("At least one Doctor Id is required.")
            .Must(ids => ids != null && ids.Any()).WithMessage("At least one Doctor Id must be provided.");

        RuleForEach(x => x.DoctorIds)
            .SetValidator(new DoctorIdValidator());

        RuleFor(x => x.UserId)
    .Must(id => string.IsNullOrEmpty(id) || Guid.TryParse(id, out _))
    .WithMessage("User Id must be a valid GUID if provided.");
    }
}

public class DoctorIdValidator : AbstractValidator<DoctorIds>
{
    public DoctorIdValidator()
    {
        RuleFor(x => x.DoctorId)
             .NotEmpty().WithMessage("Doctor Id is required.");
    }
}


