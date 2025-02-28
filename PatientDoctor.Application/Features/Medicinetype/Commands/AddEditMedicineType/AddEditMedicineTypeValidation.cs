using FluentValidation;
namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    public class AddEditMedicineTypeValidation : AbstractValidator<AddEditMedicineTypeCommand>
    {
        public AddEditMedicineTypeValidation()
        {
            RuleFor(c => c.TypeName)
            .NotEmpty().WithMessage("{TypeName} is required.")
            .MaximumLength(50).WithMessage("{TypeName} must not exceed 50 characters.")
            .MinimumLength(3).WithMessage("{TypeName} must be at least 3 characters long.");
        }
    }
}
