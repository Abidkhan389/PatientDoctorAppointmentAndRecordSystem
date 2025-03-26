using FluentValidation;

namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription
{
    public class AddPatientDescriptoinValidation : AbstractValidator<AddPatientDescriptionCommand>
    {
        public AddPatientDescriptoinValidation()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id is required.");

            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.")
                .MaximumLength(100).WithMessage("DoctorId cannot exceed 100 characters.");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.");

            RuleFor(x => x.ComplaintOf)
                .NotEmpty().WithMessage("ComplaintOf is required.")
                .MaximumLength(500).WithMessage("ComplaintOf cannot exceed 500 characters.")
                .Matches(@"^\S.*$").WithMessage("ComplaintOf cannot contain only whitespace.");

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required.")
                .MaximumLength(500).WithMessage("Diagnosis cannot exceed 500 characters.")
                .Matches(@"^\S.*$").WithMessage("Diagnosis cannot contain only whitespace.");

            RuleFor(x => x.Plan)
                .MaximumLength(1000).WithMessage("Plan cannot exceed 1000 characters.");

            RuleFor(x => x.LeftVision)
                .MaximumLength(100).WithMessage("LeftVision cannot exceed 100 characters.");

            RuleFor(x => x.RightVision)
                .MaximumLength(100).WithMessage("RightVision cannot exceed 100 characters.");

            RuleFor(x => x.LeftMG)
                .MaximumLength(100).WithMessage("LeftMG cannot exceed 100 characters.");

            RuleFor(x => x.RightMG)
                .MaximumLength(100).WithMessage("RightMG cannot exceed 100 characters.");

            RuleFor(x => x.LeftEOM)
                .MaximumLength(100).WithMessage("LeftEOM cannot exceed 100 characters.");

            RuleFor(x => x.RightEom)
                .MaximumLength(100).WithMessage("RightEom cannot exceed 100 characters.");

            RuleFor(x => x.LeftOrtho)
                .MaximumLength(100).WithMessage("LeftOrtho cannot exceed 100 characters.");

            RuleFor(x => x.RightOrtho)
                .MaximumLength(100).WithMessage("RightOrtho cannot exceed 100 characters.");

            RuleFor(x => x.LeftTension)
                .MaximumLength(100).WithMessage("LeftTension cannot exceed 100 characters.");

            RuleFor(x => x.RightTension)
                .MaximumLength(100).WithMessage("RightTension cannot exceed 100 characters.");

            RuleFor(x => x.LeftAntSegment)
                .MaximumLength(100).WithMessage("LeftAntSegment cannot exceed 100 characters.");

            RuleFor(x => x.RightAntSegment)
                .MaximumLength(100).WithMessage("RightAntSegment cannot exceed 100 characters.");

            RuleFor(x => x.LeftDisc)
                .MaximumLength(100).WithMessage("LeftDisc cannot exceed 100 characters.");

            RuleFor(x => x.RightDisc)
                .MaximumLength(100).WithMessage("RightDisc cannot exceed 100 characters.");

            RuleFor(x => x.LeftMacula)
                .MaximumLength(100).WithMessage("LeftMacula cannot exceed 100 characters.");

            RuleFor(x => x.RightMacula)
                .MaximumLength(100).WithMessage("RightMacula cannot exceed 100 characters.");

            RuleFor(x => x.LeftPeriphery)
                .MaximumLength(100).WithMessage("LeftPeriphery cannot exceed 100 characters.");

            RuleFor(x => x.RightPeriphery)
                .MaximumLength(100).WithMessage("RightPeriphery cannot exceed 100 characters.");

            // Medicine List Validations
            RuleFor(x => x.medicine)
                .NotNull().WithMessage("Medicine list is required.")
                .Must(x => x.Any()).WithMessage("At least one medicine must be provided.")
                .ForEach(x =>
                {
                    x.SetValidator(new MedicineOptionsValidator());
                });
        }
    }

    public class MedicineOptionsValidator : AbstractValidator<MedicineOptions>
    {
        public MedicineOptionsValidator()
        {
            RuleFor(x => x.MedicineId)
                .NotEmpty().WithMessage("MedicineId is required.");


            RuleFor(x => x.DurationInDays)
                .GreaterThan(0).WithMessage("DurationInDays must be greater than zero.");

            RuleFor(x => x)
                .Must(med =>
                    med.Morning == true ||
                    med.Afternoon == true ||
                    med.Evening == true ||
                    med.RepeatEveryHours == true ||
                    med.RepeatEveryTwoHours ==true ||
                    med.Night == true)
                .WithMessage("At least one dose time (Morning, Afternoon, Evening, Night,RepeatEveryHours,RepeatEveryTwoHours) must be selected.");
        }
    }
}
