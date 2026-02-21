
namespace PatientDoctor.API.Endpoints;
public class updatePatientAppointmentStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/UpdatePatientAppointmentStatus", async (UpdatePatientAppointmentStatusCommand command, IMediator mediator) =>
        {
            return await mediator.Send(command);
        }).RequireAuthorization(Roles.DoctorAssistant,Roles.Admin);
    }
}

