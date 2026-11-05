namespace PatientDoctor.API.Controllers;
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        public PatientController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActivePatients model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("AddPatientDescription")]
        public async Task<object> AddPatientDescription(AddPatientDescriptionCommand model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("AddEditPatient")]
        public async Task<object> AddEditPatient(AddEditPatientCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new AddEditPatientWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<object> GetAllByProc(GetPatientList model)
        {
            var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new GetPatientListWithUser(model, DocterId));
            //return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetAllTodeyPatientAppoitments")]
        public async Task<object> GetAllTodeyPatientAppoitments(GetPatientAppoitmentsList model)
        {
            var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new GetPatientAppoitmentListWithDocter(model, DocterId));
            //return await _mediator.Send(model);
        }
        [HttpGet]
        [Route("GetPatientById")]
        public async Task<object> GetPatientById(Guid PatientId)
        {
            GetPatientById patientobj = new GetPatientById();
            patientobj.Id = PatientId;
            return await _mediator.Send(patientobj);
        }

        [HttpGet]
        [Route("GetPatientDescriptionById")]
        public async Task<object> GetPatientDescriptionById(Guid PatientId)
        {
            GetPatientDescription patientobj = new GetPatientDescription();
            patientobj.PatientId = PatientId;
            return await _mediator.Send(patientobj);
        }
        [HttpPost("GetPatientRecordListWithDoctor")]
        public async Task<Object> GetPatientRecordListWithDoctor(GetPatientRecordList model)
        {
            var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new GetPatientRecordListWithDoctor(model, DocterId));
        }
        
        [HttpPost("GetPatientDetailForPdf")]
        public async Task<Object> GetPatientDetailForPdf(GetPatientDetailsForPdfRequest model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost("GetDoctorAppointmentsSlotsOfDay")]
        public async Task<Object> GetDoctorAppointmentsSlotsOfDay(GetDoctorTimeSlotsByDayIdAndDoctorId model)
        {
            return await _mediator.Send(model);
        }

        [HttpPost("PatientDiscount")]
        public async Task<Object> PatientDiscount(PatientDiscount model)
        {
            return await _mediator.Send(model);
        }

    }

