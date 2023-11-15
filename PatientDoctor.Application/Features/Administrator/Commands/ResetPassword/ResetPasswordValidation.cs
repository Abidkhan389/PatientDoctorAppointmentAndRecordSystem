using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Administrator.Commands.ResetPassword
{

    public class ResetPasswordValidation : AbstractValidator<ResetPasswordCommand>
    {
            public ResetPasswordValidation()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required.");

                RuleFor(x => x.OldPassword)
                    .NotEmpty().WithMessage("OldPassword is required.")
                    .MinimumLength(4).WithMessage("OldPassword must be at least 4 characters.")
                    .Must(oldPassword => !string.IsNullOrWhiteSpace(oldPassword)).WithMessage("OldPassword cannot contain only white spaces.")
                    .Must(oldPassword => !oldPassword.Contains(" ")).WithMessage("OldPassword cannot contain white spaces.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(4).WithMessage("Password must be at least 4 characters.")
                    .Must(password => !string.IsNullOrWhiteSpace(password)).WithMessage("Password cannot contain only white spaces.")
                    .Must(password => !password.Contains(" ")).WithMessage("Password cannot contain white spaces.");

                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithMessage("ConfirmPassword is required.")
                    .Equal(x => x.Password).WithMessage("ConfirmPassword should match the Password.")
                    .Must(confirmPassword => !string.IsNullOrWhiteSpace(confirmPassword)).WithMessage("ConfirmPassword cannot contain only white spaces.")
                    .Must(confirmPassword => !confirmPassword.Contains(" ")).WithMessage("ConfirmPassword cannot contain white spaces.");
            }
    }

}
