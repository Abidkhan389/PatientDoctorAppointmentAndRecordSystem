using FluentValidation;
using System.Text.RegularExpressions;
namespace PatientDoctor.Application.Features.Administrator.Quries;
public class GetUserProfileByEmailAndIdValidations : AbstractValidator<GetUserProfileByEmailAndId>
{
    public GetUserProfileByEmailAndIdValidations()
    {
        RuleFor(x => x.EmailOrPhoneNumber)
            .NotEmpty().WithMessage("Email or Phone number is required.")
            .NotNull().WithMessage("Email or Phone number must not be null.")
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Email or Phone number cannot be whitespace.")
            .Must(IsValidEmailOrPhone).WithMessage("Must be a valid email or phone number.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .NotNull().WithMessage("UserId must not be null.")
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("UserId cannot be whitespace.");
    }

    private bool IsValidEmailOrPhone(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Check for valid email
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (emailRegex.IsMatch(input))
            return true;

        // Check for valid phone number (basic 10-15 digit numeric check)
        var phoneRegex = new Regex(@"^\d{10,15}$");
        if (phoneRegex.IsMatch(input))
            return true;

        return false;
    }
}

