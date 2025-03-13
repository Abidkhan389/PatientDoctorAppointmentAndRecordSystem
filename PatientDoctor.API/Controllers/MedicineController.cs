using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.DoctorMedicine.Command;
using PatientDoctor.Application.Features.DoctorMedicine.Query;
using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicinePotency;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicineTypes;
using PatientDoctor.Application.Features.Medicine.Quries.GetById;
using PatientDoctor.Application.Features.Medicinetype.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;
        public MedicineController(IMediator mediator, IResponse response)
        {
            this._mediator = mediator;
            this._response = response;
        }
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<object> ActiveInActive([FromBody] ActiveInActiveMedicine model)
        {
            return await _mediator.Send(model);
        }
        [HttpPost]
        [Route("AddEditmedicine")]
        public async Task<object> AddEditmedicine(AddEditMedicineCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            return await _mediator.Send(new AddEditMedicineWithUserId(model, UserId));
        }
        [HttpPost]
        [Route("GetAllByProc")]
        public async Task<Object> GetAllByProc(GetMedicineList model)
        {
            var result= await _mediator.Send(model);
            return result;
        }
        [HttpPost]
        [Route("GetMedicineById")]
        public async Task<object> GetMedicineById(GetMedicineById MedicineId)
        {
            return await _mediator.Send(MedicineId);
        }
        [HttpGet]
        [Route("GetMedicinePotencyByMedicineTypeId")]
        public async Task<object> GetMedicinePotencyByMedicineTypeId([FromQuery]  GetAllMedicinePotencyByMedicineTypeId MedicineId)
        {
            return await _mediator.Send(MedicineId);
        }
        [HttpGet]
        [Route("GetMedicineTypesList")]
        public async Task<object> GetMedicineTypesList()
        {
            return await _mediator.Send(new GetAllMedicineTypes());
        }
        [HttpPost]
        [Route("GetMedicineList")]
        public async Task<object> GetMedicineList(GetAllMedicines model)
        {
            return await _mediator.Send(model);
        }

        [HttpGet]
        [Route("GetDoctorMedicineMappingList")]
        public async Task<object> GetDoctorMedicineMappingList([FromQuery] Guid medicineId)
        {
            return await _mediator.Send(new DoctorMedicineById(medicineId));
        }
        [HttpPost]
        [Route("CreateDoctorMedicineMapping")]
        public async Task<object> CreateDoctorMedicineMapping(AddEditDoctorMedicineCommand model)
        {
            var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
            model.UserId = UserId.ToString();
            return await _mediator.Send(model);
        }
    }
}
