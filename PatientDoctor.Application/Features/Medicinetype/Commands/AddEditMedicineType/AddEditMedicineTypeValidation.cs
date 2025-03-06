using FluentValidation;
namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    public class AddEditMedicineTypeValidation : AbstractValidator<AddEditMedicineTypeCommand>
    {
        public AddEditMedicineTypeValidation()
        {
            RuleFor(c => c.TypeName)
                .NotEmpty().WithMessage("{PropertyName} is required.") // Null ya empty check karega
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("{PropertyName} cannot be only whitespace.") // White spaces check karega
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters long.");
        }

    }
}
