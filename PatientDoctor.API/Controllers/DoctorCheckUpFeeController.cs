using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorCheckUpFeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        public DoctorCheckUpFeeController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveDoctorCheckupFee model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("AddEditDoctorCheckUpFee")]
        public async Task<object> AddEditDoctorCheckUpFee(AddEditDoctorCheckUpFeeCommands model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new DoctorCheckUpFeeWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<Object> GetAllByProc(GetDoctorCheckUpFeeDetailsList model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetDoctorCheckUpFeeById")]
        public async Task<object> GetDoctorCheckUpFeeById(GetDocterCheckupFeeById doctorCheckUpFeeId)
        {
            return await _mediator.Send(doctorCheckUpFeeId);
        }
    }
}
