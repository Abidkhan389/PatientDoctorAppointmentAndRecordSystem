using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.Commands.ActiveInActive;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;
namespace PatientDoctor.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientCheckUpHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        public PatientCheckUpHistoryController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<object> GetAllByProc(GetPatientCheckUpHistryList model)
        {
            var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);

            return await _mediator.Send(new GetAllPatientCheckUpHistroyByDoctor(model, DocterId));
            //return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActivePatientCheckUpHistory model)
        {
            return await _mediator.Send(model);
        }
    }
}
    


