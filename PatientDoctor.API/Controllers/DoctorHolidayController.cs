
namespace PatientDoctor.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DoctorHolidayController(IMediator _mediator, IResponse _response) : ControllerBase
{
    [HttpPost]
    [Route("ActiveInActive")]
    public async Task<object> ActiveInActive([FromBody] ActiveInActiveDoctorHoliday model)
    {
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("AddEditDoctorHoliday")]
    public async Task<object> AddEditDoctorHoliday(AddEditDoctorHolidayCommand model)
    {
        var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
        model.LogedInUserId = UserId.ToString();
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetAllByProc")]
    public async Task<object> GetAllByProc(GetDoctorHolidayList model)
    {
        var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
        model.LogedInDoctorId = DocterId.ToString();
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetByIdDoctorHoliday")]
    public async Task<object> GetByIdDoctorHoliday(GetByIdDoctorHoliday model)
    {
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetDoctorHolidayByDoctorIdForPatientAppointment")]
    public async Task<object> GetDoctorHolidayByDoctorIdForPatientAppointment
                                    (GetDoctorHolidayByDoctorIdForPatientAppointment model)
    {
        return await _mediator.Send(model);
    }
}
