using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Dashboard.Quries;
using PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        public WelcomeController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("CurrentWeekMonthWeekPatientCount")]
        public async Task<object> ActiveInActive([FromBody] WelComeCurrentWeekAndMonth model)
        {
            var loggedInUserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            model.logInUserId = Convert.ToString(loggedInUserId);
            return await _mediator.Send(model);

        }
    }
}
