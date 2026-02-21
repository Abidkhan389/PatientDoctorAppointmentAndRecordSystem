namespace PatientDoctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteAppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        private readonly IIdentityRepository identityRepository;
        public WebsiteAppointmentController(IMediator mediator, IResponse response, IIdentityRepository identityRepository)
        {
            this._mediator = mediator;
            this._response = response;
            this.identityRepository = identityRepository;
        }
        [HttpGet]
        [Route("GetAllDoctors")]
        public async Task<object> GetAllDoctors()
        {
            return await identityRepository.GetAllDoctors();
        }
        [HttpGet]
        [Route("GetDoctorFeeByDocotorId")]
        public async Task<object> GetDoctorFeeByDocotorId(string DoctorId)
        {
            return await _mediator.Send(new GetDoctorFee(DoctorId));
        }
        [HttpPost]
        [Route("AddEditPatient")]
        public async Task<object> AddEditPatient(AddEditPatientCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new AddEditPatientWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetDoctorHolidayByDoctorIdForPatientAppointment")]
        public async Task<object> GetDoctorHolidayByDoctorIdForPatientAppointment
                                    (GetDoctorHolidayByDoctorIdForPatientAppointment model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost("GetDoctorAppointmentsSlotsOfDay")]
        public async Task<Object> GetDoctorAppointmentsSlotsOfDay(GetDoctorTimeSlotsByDayIdAndDoctorId model)
        {
            return await _mediator.Send(model);
        }
    }
}
