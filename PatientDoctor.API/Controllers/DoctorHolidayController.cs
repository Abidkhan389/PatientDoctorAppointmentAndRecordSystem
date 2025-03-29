using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DoctorHolidayController(IMediator _mediator, IResponse _response) : ControllerBase
{
    [HttpPost]
    [Route("ActiveInActive")]
    public async Task<object> ActiveInActive([FromBody] ActiveInActiveDoctorHoliday model)
    {
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("AddEditDoctorHoliday")]
    public async Task<object> AddEditDoctorHoliday(AddEditDoctorHolidayCommand model)
    {
        var UserId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
        model.LogedInUserId = UserId.ToString();
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetAllByProc")]
    public async Task<object> GetAllByProc(GetDoctorHolidayList model)
    {
        var DocterId = HelperStatic.GetUserIdFromClaims((ClaimsIdentity)User.Identity);
        model.LogedInDoctorId = DocterId.ToString();
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetByIdDoctorHoliday")]
    public async Task<object> GetByIdDoctorHoliday(GetByIdDoctorHoliday model)
    {
        return await _mediator.Send(model);
    }
    [HttpPost]
    [Route("GetDoctorHolidayByDoctorIdForPatientAppointment")]
    public async Task<object> GetDoctorHolidayByDoctorIdForPatientAppointment
                                    (GetDoctorHolidayByDoctorIdForPatientAppointment model)
    {
        return await _mediator.Send(model);
    }
}
