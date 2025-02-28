using FluentValidation;

namespace PatientDoctor.Application.Features.Administrator.Commands.Register;
    public class UserRegisterValidation : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterValidation()
        {
            RuleFor(x => x.Uname)
                .NotEmpty().WithMessage("User Name is required")
                .MinimumLength(6).WithMessage("Minimum length is 6 characters");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Must(ValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");

            RuleFor(x => x.cpassword)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(x => x.password).WithMessage("Passwords do not match");
        }

        private bool ValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            // Ensure password contains at least one uppercase, one lowercase, one digit, and one special character
            return password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*()_+-=[]{}|;:'\",.<>?/`~".Contains(ch));
        }
    }


