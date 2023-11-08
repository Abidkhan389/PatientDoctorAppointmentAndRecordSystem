using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Dashboard.Quries;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;

        public DashboardController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpGet]
        [Route("GetOverViewForAdminDashboard")]
        public async Task<object> GetOverViewForAdminDashboard()
        {
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            DashboardOverView dashboardOverView = new DashboardOverView();
            return await _mediator.Send(dashboardOverView);

        }

    }
}
