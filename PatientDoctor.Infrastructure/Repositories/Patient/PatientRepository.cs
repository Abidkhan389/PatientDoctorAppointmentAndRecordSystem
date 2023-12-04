using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Application.Features.Patient.Quries;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using System.Diagnostics.Eventing.Reader;
using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using System.Xml;
using System.Text.Json;

namespace PatientDoctor.Infrastructure.Repositories.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountResponse _countResp;
        private readonly IConfiguration _configuration;
        private readonly ICryptoService _crypto;

        public PatientRepository(DocterPatiendDbContext context,
            IResponse response, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, ICountResponse countResp,
            IConfiguration configurations, ICryptoService crypto)
        {
            this._context = context;
            this._response = response;
            this._userManager = userManager;
            this._countResp = countResp;
            this._configuration = configurations;
            this._crypto = crypto;
        }
        private IResponse CreateSuccessResponse(string message)
        {
            return new Response { Success = Constants.ResponseSuccess, Message = message };
        }

        private IResponse CreateErrorResponse(string message)
        {
            return new Response { Success = Constants.ResponseFailure, Message = message };
        }

        private async Task<bool> IsAppointmentTimeConflictAsync(string doctorId, DateTime appointmentTime)
        {
            var minimumAllowedTime = appointmentTime.AddMinutes(Convert.ToInt32(_configuration["PatientSettings:PatientAllowedTime"]));
            var existingAppointment = await _context.Appointment
                .Where(x => x.DoctorId == doctorId &&
                            x.AppointmentDate.Date == appointmentTime.Date &&
                            x.AppointmentDate == appointmentTime &&
                            x.AppointmentDate >= minimumAllowedTime)
                .FirstOrDefaultAsync();

            return existingAppointment != null;
        }
        public async Task<IResponse> ActiveInActive(ActiveInActivePatients model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var patient = await _context.Patient.FindAsync(model.Id);
                if (patient == null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "patient");
                    _response.Success = Constants.ResponseFailure;
                }
                patient.Status = model.Status;
                _context.Patient.Update(patient);
                var patiendetial= await _context.PatientDetails.Where(x=> x.PatientId==model.Id).FirstOrDefaultAsync();
                patiendetial.Status= model.Status;
                _context.PatientDetails.Update(patiendetial);
                await transaction.CommitAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
            }
            return _response;
        }
        public async Task<IResponse> AddEditPatient(AddEditPatientWithUserId model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (model.AddEditPatientObj.PatientId == null)
                {
                    //var patient = await _context.Patient.SingleOrDefaultAsync(x => x.Cnic == model.AddEditPatientObj.Cnic);

                    //if (patient != null)
                    //{
                    //    var appointmentCount = await _context.Appointment
                    //        .Where(a => a.PatientId == patient.PatientId &&
                    //                    a.DoctorId == model.AddEditPatientObj.DoctorId &&
                    //                    a.AppointmentDate.Date == model.AddEditPatientObj.AppoitmentTime.Date)
                    //        .CountAsync();

                    //    if (appointmentCount > 2)
                    //    {
                    //        return CreateErrorResponse("You can't take more than two appointments in a day from the same doctor.");
                    //    }
                    //}

                    // Check for an existing appointment within 30 minutes of the selected time
                    if (await IsAppointmentTimeConflictAsync(model.AddEditPatientObj.DoctorId, model.AddEditPatientObj.AppoitmentTime))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 30 minutes after the existing appointment from the same doctor.");
                    }

                    // Create new patient, patient details, and appointment records
                    var patientObj = new PatientDoctor.domain.Entities.Patient
                    {
                        PatientId = Guid.NewGuid(),
                        FirstName = model.AddEditPatientObj.FirstName,
                        LastName = model.AddEditPatientObj.LastName,
                        Status = 1,
                        Cnic = model.AddEditPatientObj.Cnic,
                        Gender = model.AddEditPatientObj.Gender,
                        DoctoerId = model.AddEditPatientObj.DoctorId,
                        DateofBirth = model.AddEditPatientObj.DateofBirth,
                        Description="",
                    };
                    await _context.Patient.AddAsync(patientObj);

                    var patientDetails = new PatientDetails
                    {
                        PatiendDetailsId = Guid.NewGuid(),
                        PatientId = patientObj.PatientId,
                        PhoneNumber = model.AddEditPatientObj.PhoneNumber,
                        City = model.AddEditPatientObj.City,
                        BloodType = model.AddEditPatientObj.BloodType,
                        Status = 1,
                        MaritalStatus = model.AddEditPatientObj.MaritalStatus,
                        CheckUpStatus = 0, // 0 for bydefault waiting, means patient is in waiting list
                        CreatedBy=model.UserId,
                        CreatedOn=DateTime.Now,
                    };
                    await _context.PatientDetails.AddAsync(patientDetails);
                    var patientAppointment = new Appointment
                    {
                        AppointmentId = Guid.NewGuid(),
                        DoctorId = model.AddEditPatientObj.DoctorId,
                        PatientId = patientObj.PatientId,
                        AppointmentDate = model.AddEditPatientObj.AppoitmentTime,
                        PatientDetailsId=patientDetails.PatiendDetailsId
                    };
                    await _context.Appointment.AddAsync(patientAppointment);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreateSuccessResponse(Constants.DataSaved);
                }
                else
                {
                    // Handle editing an existing patient's appointment (similar logic as above)
                    var patient = await _context.Patient.FindAsync(model.AddEditPatientObj.PatientId);

                    if (patient == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Patient"));
                    }
                    var appointmentCount = await _context.Appointment
                       .Where(a => a.PatientId == patient.PatientId && a.DoctorId == patient.DoctoerId && a.AppointmentDate.Date == model.AddEditPatientObj.AppoitmentTime.Date)
                       .CountAsync();
                    if (appointmentCount > 2)
                    {
                        // Handle the case where the patient has already taken two appointments.
                        return CreateErrorResponse("You can't take more than two appointments in a day from the same doctor.");
                    }
                    // Check for an existing appointment within 30 minutes of the selected time
                    if (await IsAppointmentTimeConflictAsync(model.AddEditPatientObj.DoctorId, model.AddEditPatientObj.AppoitmentTime))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 30 minutes after the existing appointment from the same doctor.");
                    }
                    // Update existing patient and appointment records
                    patient.FirstName = model.AddEditPatientObj.FirstName;
                    patient.LastName = model.AddEditPatientObj.LastName;
                    patient.Cnic = model.AddEditPatientObj.Cnic;
                    patient.Gender = model.AddEditPatientObj.Gender;
                    patient.DoctoerId = model.AddEditPatientObj.DoctorId;
                    patient.DateofBirth = model.AddEditPatientObj.DateofBirth;

                    var patientDetails = await _context.PatientDetails.FindAsync(patient.PatientId);
                    if (patientDetails == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Patient Details"));
                    }

                    patientDetails.PhoneNumber = model.AddEditPatientObj.PhoneNumber;
                    patientDetails.City = model.AddEditPatientObj.City;
                    patientDetails.BloodType = model.AddEditPatientObj.BloodType;
                    patientDetails.MaritalStatus = model.AddEditPatientObj.MaritalStatus;
                    patientDetails.UpdatedBy = model.UserId;
                    patientDetails.UpdatedOn=DateTime.Now;
                    var existingAppointment = await _context.Appointment
                        .Where(x => x.PatientId == patient.PatientId && x.PatientDetailsId == patientDetails.PatiendDetailsId)
                        .FirstOrDefaultAsync();

                    if (existingAppointment == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Appointment"));
                    }

                    existingAppointment.DoctorId = model.AddEditPatientObj.DoctorId;
                    existingAppointment.AppointmentDate = model.AddEditPatientObj.AppoitmentTime;

                    // Update tables
                    _context.Patient.Update(patient);
                    _context.PatientDetails.Update(patientDetails);
                    _context.Appointment.Update(existingAppointment);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreateSuccessResponse(Constants.DataUpdate);

                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
                return CreateErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetAllByProc(GetPatientList model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "FullName" : model.Sort;
            var data=(from patient in _context.Patient
                      join main in _context.Users on patient.DoctoerId equals main.Id
                      join p_details in _context.PatientDetails on patient.PatientId equals p_details.PatientId
                      join App in _context.Appointment on patient.PatientId equals App.PatientId
                      where (
                                (EF.Functions.ILike(patient.FirstName, $"%{model.PatientName}%") || string.IsNullOrEmpty(model.PatientName)) 
                              && (EF.Functions.ILike(p_details.City, $"%{model.City}%") || string.IsNullOrEmpty(model.City))
                              && (EF.Functions.ILike(patient.Cnic, $"%{model.Cnic}%") || string.IsNullOrEmpty(model.Cnic))
                              && (EF.Functions.ILike(p_details.PhoneNumber, $"%{model.MobileNumber}%") || string.IsNullOrEmpty(model.MobileNumber))
                            //(model.DoctorId == null || patient.DoctoerId == model.DoctorId)
                            )
                      select new VM_Patient
                           {
                               PatientId = patient.PatientId,
                               FullName=patient.FirstName+patient.LastName,
                               Gender=patient.Gender,
                               DoctorName=main.UserName,
                               AppointmentTime = App.AppointmentDate,
                               PatientPhoneNumber=p_details.PhoneNumber,
                               DoctorPhoneNumber=main.PhoneNumber,
                               City=p_details.City,
                               BloodType=p_details.BloodType,
                               Cnic=patient.Cnic,
                               Status=patient.Status,
                               MaritalStatus=p_details.MaritalStatus,
                               CheckUpStatus = p_details.CheckUpStatus
                           }).AsQueryable();


            var count = data.Count();
            var sorted = await HelperStatic.OrderBy(data, model.SortEx, model.OrderEx == "desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = _countResp;
            return _response;
        }
        public async Task<IResponse> GetPatientById(GetPatientById model)
        {
            var patientobj = await (from main in _userManager.Users
                                    join patient in _context.Patient on main.Id equals patient.DoctoerId
                                    join p_Details in _context.PatientDetails on patient.PatientId equals p_Details.PatientId
                                    join app in _context.Appointment on patient.PatientId equals app.PatientId
                                    where (patient.Status == 1
                                        && main.Id == patient.DoctoerId
                                        && patient.PatientId == model.Id
                                        && (app.PatientId == patient.PatientId && app.PatientDetailsId == p_Details.PatiendDetailsId))
                                    select new VM_PatientById
                                    {
                                        PatientId = patient.PatientId,
                                        DoctorId = main.Id,
                                        PhoneNumber = p_Details.PhoneNumber,
                                        FirstName = patient.FirstName,
                                        LastName = patient.LastName,
                                        City = p_Details.City,
                                        Cnic = patient.Cnic,
                                        BloodType = p_Details.BloodType,
                                        MaritalStatus = p_Details.MaritalStatus,
                                        Gender = patient.Gender,
                                        DateofBirth = patient.DateofBirth,
                                        AppoitmentTime = app.AppointmentDate
                                    }).FirstOrDefaultAsync();
            if (patientobj != null)
            {
                _response.Data = patientobj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "user");
            }
            return _response;

        }

        public async Task<IResponse> AddPatientDescription(AddPatientDescriptionCommand model)
        {
            var patient = await _context.Patient
            .Where(x => x.PatientId == model.PatientId)
            .FirstOrDefaultAsync();
            var patientDetails = await _context.PatientDetails
                .FirstOrDefaultAsync(pd => pd.PatientId == model.PatientId);
            if (patientDetails != null && patient != null)
            {
                patient.Description= JsonSerializer.Serialize(model);
                patientDetails.CheckUpStatus = 1; // update check status to 1, its means patient is checked
                _context.Patient.Update(patient);
                _context.PatientDetails.Update(patientDetails);
                await _context.SaveChangesAsync();
                _response.Success= Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
            else
            {
                _response.Success= Constants.ResponseFailure;
                _response.Message = Constants.NotFound;
            }
            return _response;
        }

        public async Task<IResponse> GetAllPatientAppoitmentWithDoctorProc(GetPatientAppoitmentListWithDocter model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "FullName" : model.Sort;
            var data = (from patient in _context.Patient
                        join main in _context.Users on patient.DoctoerId equals main.Id
                        join p_details in _context.PatientDetails on patient.PatientId equals p_details.PatientId
                        join App in _context.Appointment on patient.PatientId equals App.PatientId
                        where (
                                  (EF.Functions.ILike(patient.FirstName, $"%{model.GetPatientAppoitmentsListObj.PatientName}%") || string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.PatientName))
                                && (EF.Functions.ILike(p_details.City, $"%{model.GetPatientAppoitmentsListObj.City}%") || string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.City))
                                && (EF.Functions.ILike(patient.Cnic, $"%{model.GetPatientAppoitmentsListObj.Cnic}%") || string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.Cnic))
                                && (EF.Functions.ILike(p_details.PhoneNumber, $"%{model.GetPatientAppoitmentsListObj.MobileNumber}%") || string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.MobileNumber))
                                &&(App.DoctorId == model.DocterId && App.PatientId==patient.PatientId&&p_details.PatiendDetailsId==App.PatientDetailsId
                                && App.AppointmentDate.Date==model.GetPatientAppoitmentsListObj.Todeydatetime.Date)
                              )
                        select new VM_Patient
                        {
                            PatientId = patient.PatientId,
                            FullName = patient.FirstName + " " + patient.LastName,
                            Gender = patient.Gender,
                            DoctorName = main.UserName,
                            AppointmentTime = App.AppointmentDate,
                            PatientPhoneNumber = p_details.PhoneNumber,
                            DoctorPhoneNumber = main.PhoneNumber,
                            City = p_details.City,
                            BloodType = p_details.BloodType,
                            Cnic = patient.Cnic,
                            Status = patient.Status,
                            MaritalStatus = p_details.MaritalStatus,
                            CheckUpStatus = p_details.CheckUpStatus
                        }).AsQueryable();
            var count = data.Count();
            var sorted = await HelperStatic.OrderBy(data, model.SortEx, model.OrderEx == "desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = _countResp;
            return _response;
        }

        public async Task<IResponse> GetPatientDescriptionById(GetPatientDescription model)
        {
            var patientDescriptionEntity = await _context.Patient
                                          .Where(x => x.PatientId == model.PatientId)
                                          .FirstOrDefaultAsync();
            if (patientDescriptionEntity != null && patientDescriptionEntity.Description != null)
            { 
                var patientDescription= JsonSerializer.Deserialize<AddPatientDescriptionCommand>(patientDescriptionEntity.Description);
                _response.Data = patientDescription;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound;
            }
            return _response;
        }
    }
}
