namespace PatientDoctor.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController(IMediator _mediator, IResponse _response) : ControllerBase
    {
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveDoctorAvailability model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("AddEditDoctorAvaibality")]
        public async Task<object> AddEditDoctorAvaibality(AddEditDoctorAvailabilityCommands model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new AddEditDoctorAvailabilityWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<Object> GetAllByProc(GetDoctorAvailabiltiesList model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            model.UserId = UserId.ToString();
            return await _mediator.Send(model);
        }
        [HttpGet]
        [Route("GetByIdDoctorAvaibality")]
        public async Task<IActionResult> GetByIdDoctorAvaibality([FromQuery] Guid Id)
        {
            var result = await _mediator.Send(new GetByIdDoctorAvailabiliteis(Id));
            return Ok(result);
        }

    }
}
