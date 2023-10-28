using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
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
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            return await _mediator.Send(model);

        }
        [HttpPost]
        [Route("AddEditPatient")]
        public async Task<object> AddEditPatient(AddEditPatientCommand model)
        {
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<object> GetAllByProc(GetPatientList model)
        {
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new GetPatientListWithDocterId(model, DocterId));
        }
        [HttpGet]
        [Route("GetPatientById")]
        public async Task<object> GetUserById(GetPatientById PatientId)
        {
            if (PatientId == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            return await _mediator.Send(PatientId);

        }
    }
}
