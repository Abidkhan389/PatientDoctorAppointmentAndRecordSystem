using System;
using FluentValidation;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees
{
	public class AddEditDoctorCheckUpFeeValidation : AbstractValidator<AddEditDoctorCheckUpFeeCommands>
    {
		public AddEditDoctorCheckUpFeeValidation()
		{
            RuleFor(x => x.Id)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("Id is required and white space is not allowed");
            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage("Doctor ID is required.");
            RuleFor(x => x.DoctorFee)
                .NotEmpty()
                .WithMessage("Doctor ID is required.")
                .GreaterThan(300).WithMessage("Fee Should be Greater than 300");

        }
    }
}

