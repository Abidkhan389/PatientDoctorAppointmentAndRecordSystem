using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserValidation : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidation()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("{Email} is Required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Email} must not exceed 100 characters.")
                .MinimumLength(10).WithMessage("{Email} must be greater than 10 characters.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$")
                .WithMessage("{Email} is not in a valid email format.");
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("{Email} is Required.")
                .NotNull()
                .MaximumLength(60).WithMessage("{Password} must not exceed 60 characters.")
                .MinimumLength(4).WithMessage("{Password} must be greater than 4 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$")
                .WithMessage("{Password} must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.");

        }
    }
}
