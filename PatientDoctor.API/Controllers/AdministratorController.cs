using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.Administrator.Commands.Register;
using PatientDoctor.Application.Features.Administrator.Commands.UserProfile;
using PatientDoctor.Application.Features.Administrator.Quries;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdministratorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<Object> UpdateUserProfile(UserProfileCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            model.LoedInUserId = UserId;
            return await _mediator.Send(model);
        }
        
        [HttpPost]
        [Route("UserRegister")]
        public async Task<Object> UserRegister(UserRegisterCommand model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetUserProfileByEmailAndId")]
        public async Task<Object> GetUserProfileByEmailAndId(GetUserProfileByEmailAndId model)
        {
            return await _mediator.Send(model);
        }

    }
}
