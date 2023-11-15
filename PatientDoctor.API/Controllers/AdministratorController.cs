using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Persistance.ISecurity;
using PatientDoctor.Application.Features.Administrator.Commands.ResetPassword;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;

namespace PatientDoctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        private readonly ILocalAuthenticationRepository _localAuthenticationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorController(IMediator mediator, IResponse response, ILocalAuthenticationRepository localAuthenticationRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._mediator = mediator;
            this._response = response;
            this._localAuthenticationRepository = localAuthenticationRepository;
            this._userManager = userManager;
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<Object> resetPassword(ResetPasswordCommand model)
        {
            if (!ModelState.IsValid)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ModelStateStateIsInvalid;
                return Ok(_response);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message ="User With this Id is not Found";
                return Ok(_response);
            }
            var userObj = await _localAuthenticationRepository.ResolveUser(user.Email, model.OldPassword, false);
            if (userObj)
            {
                return await _mediator.Send(model);
            }
            _response.Message = ("No Password Match");
            _response.Success = Constants.ResponseFailure;
            return _response;
        }
    }
}
