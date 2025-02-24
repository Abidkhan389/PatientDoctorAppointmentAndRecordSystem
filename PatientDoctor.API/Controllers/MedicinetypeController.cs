using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
using PatientDoctor.Application.Features.Medicinetype.Quries;
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
    public class MedicinetypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public MedicinetypeController(IMediator mediator, IResponse response, IMedicinetypeRepository medicinetypeRepository)
        {
            _mediator = mediator;
            _response = response;
            _medicinetypeRepository = medicinetypeRepository;
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveMedicinetype model)
        {
            return await _mediator.Send(model);

        }
        [HttpPost]
        [Route("AddEditmedicineType")]
        public async Task<object> AddEditmedicineType(AddEditMedicineTypeCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new AddEditMedicineTypeWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<Object> GetAllByProc (GetMedicineTypeList model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("GetMedicineTypeById")]
        public async Task<object> GetMedicineTypeById(GetMedicineTypeById MedicineTypeId)
        {
            return await _mediator.Send(MedicineTypeId);
        }
        [HttpPost]
        [Route("GetAllMeDicineType")]
        public async Task<Object> GetAllMeDicineType()
        {
            return await _medicinetypeRepository.GetAllMedicineTypeWithIdAndName();
        }
    }
}
