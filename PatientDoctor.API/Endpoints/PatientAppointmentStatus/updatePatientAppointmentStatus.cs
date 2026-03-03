
namespace PatientDoctor.API.Endpoints.PatientAppointmentStatus;
public class updatePatientAppointmentStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        // Create a route group for all patient-related endpoints
        var patientGroup = app.MapGroup("/api/Patient"); // Apply global auth for patient endpoints

        // POST: /api/Patient/updatePatientAppointmentStatus
        patientGroup.MapPost("/UpdatePatientAppointmentStatus", async (UpdatePatientAppointmentStatusCommand command, IMediator mediator, HttpContext httpContext) =>
        {
            command.UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)httpContext.User.Identity);
            // Mediator sends command and gets IResponse
            var response = await mediator.Send(command);

            // Return response as object (like your other endpoints)
            return Results.Ok(response);
        })
         .RequireAuthorization("DoctorAssistantOrDoctor")
        .WithName("UpdatePatientAppointmentStatus")
        .WithSummary("Update Patient Appointment Status")
        .WithDescription("Mark a patient as checked up or update appointment status");
    }
    
}

