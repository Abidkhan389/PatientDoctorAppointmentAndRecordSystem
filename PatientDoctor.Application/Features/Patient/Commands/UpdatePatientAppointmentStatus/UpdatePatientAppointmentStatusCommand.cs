using FluentValidation;
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Commands.UpdatePatientAppointmentStatus;
public record UpdatePatientAppointmentStatusCommand : IRequest<IResponse>
{
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
}
public class UpdatePatientAppointmentStatusCommandValidator : AbstractValidator<UpdatePatientAppointmentStatusCommand>
{
    public UpdatePatientAppointmentStatusCommandValidator()
    {
        // PatientId (Guid)
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("PatientId is required.");

        // DoctorId (string)
        RuleFor(x => x.DoctorId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("DoctorId is required.")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("DoctorId cannot be empty or whitespace.");
    }

}

