using FluentValidation;

namespace PatientDoctor.Application.Features.Identity.Commands.GoogleLogin;
public class GoogleLoginValidation : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginValidation()
    {
        RuleFor(c => c.IdToken)
            .NotEmpty().WithMessage("{IdToken} is Required")
            .NotNull().WithMessage("{IdToken} is Required")
            .Must(x=> !string.IsNullOrWhiteSpace(x))
            .WithMessage("IdToken cannot be empty or whitespace");
    }
}

