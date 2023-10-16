using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Identity.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;

        public AccountController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("ActiveInactive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveIdentity model)
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
        [Route("AddEditUser")]
        public async Task<object> AddEditUser(AddEditUserCommands model)
        {
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);

            return await _mediator.Send(new AddEditUserWithCreatedOrUpdatedById(model, UserId));
        }
        [HttpPost]
        [Route("Login")]
        public async Task<object> Login(LoginUserCommand model)
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
        public async Task<object> GetAllByProc([FromBody] GetUserList model)
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
        [Route("getUserById")]
        public async Task<object> GetUserById(GetUserById UserId)
        {
            if (UserId == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            return await _mediator.Send(UserId);

        }
    }
}
