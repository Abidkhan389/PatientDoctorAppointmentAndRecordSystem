using FluentValidation;

namespace PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine
{
    public class AddEditMedicineValidation :AbstractValidator<AddEditMedicineCommand>
    {
        public AddEditMedicineValidation()
        {
            RuleFor(x=> x.Id)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Id is required and white space is not allowed");

            RuleFor(x => x.MedicineTypeId).NotEmpty().WithMessage("Medicine type ID is required.");
            RuleFor(x => x.medicineTypePotencyId).NotEmpty().WithMessage("Medicine type Ptency ID is required.");
            RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Doctor ID is required.");
            RuleFor(x => x.MedicineName)
               .NotEmpty().WithMessage("{MedicineName} is required.")
               .MaximumLength(30).WithMessage("{MedicineName} must not exceed 100 characters.")
               .MinimumLength(3).WithMessage("{MedicineName} must be at least 3 characters long.");
            RuleFor(x => x.StartingDate).NotEmpty().WithMessage("Starting date is required.")
                                         .Must(BeAValidDate).WithMessage("Starting date must be a valid date.");
            RuleFor(x => x.ExperiyDate).NotEmpty().WithMessage("Expiry date is required.")
                                        .Must(BeAValidDate).WithMessage("Expiry date must be a valid date.")
                                        .GreaterThan(x => x.StartingDate).WithMessage("Expiry date must be after the starting date.");
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
