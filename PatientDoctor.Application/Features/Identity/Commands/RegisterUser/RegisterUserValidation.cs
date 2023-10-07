using FluentValidation;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.RegisterUser
{
    public class RegisterUserValidation : AbstractValidator<AddEditUserCommands>
    {
        public RegisterUserValidation()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("{FirstName} is Required.")
                .NotNull()
                .MaximumLength(30).WithMessage("{FirstName} must not exceed 30 characters.")
                .MinimumLength(3).WithMessage("{FirstName} must be greater than 3 characters");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("{LastName} is Required")
                .NotNull()
                .MaximumLength(30).WithMessage("{LastName} must not exceed 30 characters.")
                .MinimumLength(3).WithMessage("{LastName} must be greater than 3 characters");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("{Password} is Required")
                .NotNull()
                .MaximumLength(60).WithMessage("{Password} must not exceed 60 characters.")
                .MinimumLength(4).WithMessage("{Password} must be greater than 4 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")
                .WithMessage("{Password} must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");

            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage("{RoleId} is Required")
                .NotNull()
                .WithMessage("{RoleId} must contain at least one role.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("{Email} is Required")
                .NotNull()
                .MaximumLength(100).WithMessage("{Email} must not exceed 100 characters.")
                .MinimumLength(10).WithMessage("{Email} must be greater than 10 characters.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$")
                .WithMessage("{Email} is not in a valid email format.");

            RuleFor(c => c.Cnic)
                .NotEmpty().WithMessage("{Cnic} is Required")
                .NotNull()
                .Length(13).WithMessage("{Cnic} must be exactly 13 characters.");

            RuleFor(c => c.MobileNumber)
                .NotEmpty().WithMessage("{MobileNumber} is Required")
                .NotNull()
                .Length(11).WithMessage("{MobileNumber} must be exactly 11 characters.");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("{City} is Required")
                .NotNull()
                .MaximumLength(50).WithMessage("{City} must not exceed 50 characters.")
                .MinimumLength(3).WithMessage("{City} must be greater than 3 characters.");

        }

    }

}
