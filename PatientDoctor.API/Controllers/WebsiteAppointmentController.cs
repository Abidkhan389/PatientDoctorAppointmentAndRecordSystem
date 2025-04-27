using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
using PatientDoctor.Application.Features.Identity.Quries.GetDoctorFee.GetDoctorFeeById;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

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
