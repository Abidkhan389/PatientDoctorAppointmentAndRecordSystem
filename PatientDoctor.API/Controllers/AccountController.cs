namespace PatientDoctor.API.Controllers;
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        private readonly IIdentityRepository identityRepository;

        public AccountController(IMediator mediator, IResponse response, IIdentityRepository identityRepository)
        {
            this._mediator = mediator;
            this._response = response;
            this.identityRepository = identityRepository;
        }
        [HttpPost]
        [Route("ActiveInactive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveIdentity model)
        {
            
            return await _mediator.Send(model);

        }
        [HttpPost]
        [Route("AddEditUser")]
        public async Task<object> AddEditUser(AddEditUserCommands model)
        {
            
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);

            return await _mediator.Send(new AddEditUserWithCreatedOrUpdatedById(model, UserId));
        }
        [AllowAnonymous]
        [HttpPost]
        [EnableRateLimiting("auth")]
        [Route("Login")]
        public async Task<object> Login(LoginUserCommand model)
        {
            return await _mediator.Send(model);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GoogleLogin")]
        public async Task<object> GoogleLogin(GoogleLoginCommand model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<object> GetAllByProc([FromBody] GetUserList model)
        {
            return await _mediator.Send(model); 
        }
        [HttpGet]
        [Route("GetUserById")]
        public async Task<object> GetUserById(string UserId)
        {
            GetUserById userobj= new GetUserById();
            userobj.id = UserId;
            return await _mediator.Send(userobj);
        }
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<object> GetAllRoles()
        {
            return await _mediator.Send(new GetAllRolesQuery());
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllDoctors")]
        public async Task<object> GetAllDoctors()
        {
            return await identityRepository.GetAllDoctors();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetDoctorFeeByDocotorId")]
        public async Task<object> GetDoctorFeeByDocotorId(string DoctorId)
        {
            return await _mediator.Send(new GetDoctorFee(DoctorId));
        }
        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<object> RefreshToken(RefreshTokenCommand command)
        {
            return await _mediator.Send(command);
        }
    }

