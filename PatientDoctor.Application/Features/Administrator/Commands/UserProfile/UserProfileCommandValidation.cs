using FluentValidation;
namespace PatientDoctor.Application.Features.Administrator.Commands.UserProfile;
public class UserProfileCommandValidation : AbstractValidator<UserProfileCommand>
{
    public UserProfileCommandValidation()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.EmailorPhoneNumber)
            .NotEmpty().WithMessage("Email or phone number is required.")
            .MaximumLength(100).WithMessage("Email or phone number cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.")
            .Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber)); // Validate only if provided

        RuleFor(x => x.OldPassword)
            .MinimumLength(4).WithMessage("Old password must be at least 4 characters.")
            .MaximumLength(20).WithMessage("Old password cannot exceed 20 characters.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{4,}$").WithMessage("Old password must contain at least one letter and one number.")
            .When(x => !string.IsNullOrWhiteSpace(x.OldPassword)); // Validate only if provided

        RuleFor(x => x.NewPassword)
            .MinimumLength(4).WithMessage("New password must be at least 4 characters.")
            .MaximumLength(20).WithMessage("New password cannot exceed 20 characters.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{4,}$").WithMessage("New password must contain at least one letter and one number.")
            .When(x => !string.IsNullOrWhiteSpace(x.NewPassword)); // Validate only if provided

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("New password and confirm new password must match.")
            .When(x => !string.IsNullOrWhiteSpace(x.ConfirmNewPassword)); // Validate only if provided

        RuleFor(x => x.LoedInUserId)
            .NotEmpty().WithMessage("Logged-in user ID is required.")
            .When(x => x.LoedInUserId.HasValue); // Validate only if provided

        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("Entity ID must be greater than zero.")
            .When(x => x.EntityId.HasValue); // Validate only if provided

        RuleFor(x => x.ProfilePicture)
            .MaximumLength(255).WithMessage("Profile picture path cannot exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProfilePicture)); // Validate only if provided

        RuleFor(x => x.File)
            .Must(file => file == null || file.Length > 0).WithMessage("Invalid file upload.")
            .When(x => x.File != null); // Validate only if a file is uploaded
    }
}



