namespace PatientDoctor.API.Controllers;
[Authorize(Roles = "SuperAdmin")]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReportsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    [HttpPost]
    [Route("GetCheckedPatientHistoryByDoctorReport")]
    public async Task<object> GetCheckedPatientHistoryByDoctorReport(GetCheckedPatientHistoryByDoctorQuery query)
    {
        return await _mediator.Send(query);
    }
}

