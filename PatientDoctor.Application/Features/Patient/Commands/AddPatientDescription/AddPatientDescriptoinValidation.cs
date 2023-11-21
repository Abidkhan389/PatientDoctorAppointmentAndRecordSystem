using FluentValidation;
using PatientDoctor.Application.Features.Administrator.Commands.ResetPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription
{
    public class AddPatientDescriptoinValidation : AbstractValidator<AddPatientDescriptionCommand>
    {
        public AddPatientDescriptoinValidation()
        {
            RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .MinimumLength(5).WithMessage("Description cannot less than 5 characters.")
                .Matches(@"^\S+$").WithMessage("Description cannot contain only whitespace.");

            RuleFor(x => x.Eye1).NotEmpty().WithMessage("Eye1 is required.");
            RuleFor(x => x.Eye2).NotEmpty().WithMessage("Eye2 is required.");

            RuleFor(x => x.DistanceEye1)
                .NotEmpty().WithMessage("DistanceEye1 is required.");

            RuleFor(x => x.DistanceEye2)
                .NotEmpty().WithMessage("DistanceEye2 is required.");

            RuleFor(x => x.Eye1SidePoint)
                .NotEmpty().WithMessage("Eye1SidePoint is required.")
                .Matches(@"^\S+$").WithMessage("Eye1SidePoint cannot contain only whitespace.");

            RuleFor(x => x.Eye2SidePoint)
                .NotEmpty().WithMessage("Eye2SidePoint is required.")
                .Matches(@"^\S+$").WithMessage("Eye2SidePoint cannot contain only whitespace.");

            // Custom validation for the list of medicines
            RuleFor(x => x.medicine)
                .Must(medicines => medicines != null && medicines.Any())
                .WithMessage("At least one medicine must be provided.")
                .ForEach(medicineRule => {
                    medicineRule.SetValidator(new MedicineOptionsValidator());
                });
        }
    }
    public class MedicineOptionsValidator : AbstractValidator<MedicineOptions>
    {
        public MedicineOptionsValidator()
        {
            RuleFor(x => x.MedicineTitle)
                .NotEmpty().WithMessage("MedicineTitle is required.")
                .MaximumLength(200).WithMessage("MedicineTitle cannot exceed 200 characters.")
                .MinimumLength(3).WithMessage("MedicineTitle cannot less than 3 characters.")
                .Matches(@"^\S+$").WithMessage("MedicineTitle cannot contain only whitespace.");
        }
    }
}
