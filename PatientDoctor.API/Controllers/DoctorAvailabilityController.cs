using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Doctor_Availability.Quries.GetById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

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
        [HttpPost]
        [Route("GetByIdDoctorAvaibality")]
        public async Task<object> GetByIdDoctorAvaibality(GetByIdDoctorAvailabiliteis model)
        {
            return await _mediator.Send(model);
        }
    }
}
