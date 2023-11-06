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
                var patiendetial= await _context.PatientDetails.FindAsync(model.Id);
                patiendetial.Status= model.Status;
                _context.PatientDetails.Update(patiendetial);
                await transaction.CommitAsync();
                _response.Success = Constants.ResponseSuccess;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
            }
            return _response;
        }
        public async Task<IResponse> AddEditPatient(AddEditPatientCommand model)
        {
            //using var transaction = _context.Database.BeginTransaction();
            //try
            //{
            //    if(model.PatientId == null)
            //    {
            //        var patientobj = await _context.Patient.SingleOrDefaultAsync(x => x.Cnic == model.Cnic);
            //        //var today = DateTime.Today;
            //        var appointmentCount = await _context.Appointment
            //        .Where(a => a.PatientId == patientobj.PatientId && a.DoctorId == patientobj.DoctoerId && a.AppointmentDate.Date == model.AppoitmentTime)
            //        .CountAsync();
            //        if (appointmentCount > 2)
            //        {
            //            // Handle the case where the patient has already taken two appointments.
            //            _response.Success = Constants.ResponseFailure;
            //            _response.Message = "You cant take two appoitment in a same day from a same doctor,Try with" +
            //                "Different Cnic for get more Appoitments ";
            //        }
            //        else
            //        {
            //            // Calculate the minimum allowed appointment time (30 minutes from now).
            //            var minimumAllowedTime = model.AppoitmentTime.AddMinutes(30);
            //            var existingAppointment = await _context.Appointment
            //                .Where(x => x.DoctorId == model.DoctoerId &&
            //                            x.AppointmentDate.Date == model.AppoitmentTime.Date && // Same day check
            //                            x.AppointmentDate == model.AppoitmentTime && // Existing appointment is before or at the same time
            //                            x.AppointmentDate >= minimumAllowedTime) // Existing appointment is at least 30 minutes earlier
            //                .FirstOrDefaultAsync();
            //            if (existingAppointment != null)
            //            {
            //                _response.Success = Constants.ResponseFailure;
            //                _response.Message = "Please choose an appointment time that" +
            //                    " is at least 30 minutes after the existing appointment from Same Doctor.";
            //            }
            //            else
            //            {
            //                var patientObj = new PatientDoctor.domain.Entities.Patient
            //                {
            //                    PatientId = Guid.NewGuid(),
            //                    FirstName = model.FirstName,
            //                    LastName = model.LastName,
            //                    Status = 1,
            //                    Cnic = model.Cnic,
            //                    Gender = model.Gender,
            //                    DoctoerId = model.DoctoerId,
            //                    DateofBirth = model.DateofBirth,
            //                };
            //                await _context.Patient.AddAsync(patientObj);
            //                var patientdetails = new PatientDetails
            //                {
            //                    PatiendDetailsId = Guid.NewGuid(),
            //                    PatientId = patientobj.PatientId,
            //                    PhoneNumber = model.PhoneNumber,
            //                    City = model.City,
            //                    BloodType = model.BloodType,
            //                    Status = 1,
            //                    MaritalStatus = model.MaritalStatus,
            //                };
            //                await _context.PatientDetails.AddAsync(patientdetails);
            //                var patientappoitment = new Appointment
            //                {
            //                    AppointmentId = Guid.NewGuid(),
            //                    DoctorId = model.DoctoerId,
            //                    PatientId = patientObj.PatientId,
            //                    AppointmentDate = model.AppoitmentTime
            //                };
            //                await _context.Appointment.AddAsync(patientappoitment);
            //                await _context.SaveChangesAsync();
            //                await transaction.CommitAsync();
            //                _response.Success = Constants.ResponseSuccess;
            //                _response.Message = Constants.DataSaved;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var patient= await _context.Patient.FindAsync(model.PatientId);
            //        if(patient == null)
            //        {
            //            _response.Success = Constants.ResponseFailure;
            //            _response.Message = Constants.NotFound.Replace("{data}", "Patient");
            //        }
            //        else
            //        {
            //            var appointmentCount = await _context.Appointment
            //           .Where(a => a.PatientId == patient.PatientId && a.DoctorId == patient.DoctoerId && a.AppointmentDate.Date == model.AppoitmentTime.Date)
            //           .CountAsync();
            //            if (appointmentCount > 2)
            //            {
            //                // Handle the case where the patient has already taken two appointments.
            //                _response.Success = Constants.ResponseFailure;
            //                _response.Message = "You cant take two appoitment in a day from a same doctor ";
            //            }
            //            else
            //            {
            //                var minimumAllowedTime = model.AppoitmentTime.AddMinutes(30);
            //                var existingAppointment = await _context.Appointment
            //                    .Where(x => x.DoctorId == model.DoctoerId &&
            //                                x.AppointmentDate.Date == model.AppoitmentTime.Date && // Same day check
            //                                x.AppointmentDate == model.AppoitmentTime && // Existing appointment is before or at the same time
            //                                x.AppointmentDate >= minimumAllowedTime) // Existing appointment is at least 30 minutes earlier
            //                    .FirstOrDefaultAsync();
            //                if (existingAppointment != null)
            //                {
            //                    _response.Success = Constants.ResponseFailure;
            //                    _response.Message = "Please choose an appointment time that" +
            //                        " is at least 30 minutes after the existing appointment from Same Doctor.";
            //                }
            //                else 
            //                {
            //                    patient.FirstName = model.FirstName;
            //                    patient.LastName = model.LastName;
            //                    patient.Cnic = model.Cnic;
            //                    patient.Gender = model.Gender;
            //                    patient.DoctoerId = model.DoctoerId;
            //                    patient.DateofBirth = model.DateofBirth;
            //                    var patientdetails = await _context.PatientDetails.FindAsync(patient.PatientId);
            //                    if (patientdetails == null)
            //                    {
            //                        _response.Success = Constants.ResponseFailure;
            //                        _response.Message = Constants.NotFound.Replace("{data}", "Patient Details");
            //                    }
            //                    else
            //                    {
            //                        patientdetails.PatientId = patient.PatientId;
            //                        patientdetails.PhoneNumber = model.PhoneNumber;
            //                        patientdetails.City = model.City;
            //                        patientdetails.BloodType = model.BloodType;
            //                        patientdetails.MaritalStatus = model.MaritalStatus;
            //                        var existingpatientappoitment = await _context.Appointment.
            //                            Where(x=> x.PatientId== patient.PatientId && x.PatientDetailsId== patientdetails.PatiendDetailsId).FirstOrDefaultAsync();
            //                        existingAppointment.DoctorId = model.DoctoerId;
            //                        existingAppointment.AppointmentDate = model.AppoitmentTime;
            //                        // Updating tables
            //                        _context.Patient.Update(patient);
            //                        _context.PatientDetails.Update(patientdetails);
            //                        _context.Appointment.Update(existingAppointment);
            //                        await _context.SaveChangesAsync();
            //                        await transaction.CommitAsync();
            //                        _response.Success = Constants.ResponseSuccess;
            //                        _response.Message = Constants.DataUpdate;
            //                    }
            //                }

            //            }

            //        }
            //    }
            //    return _response;
            //}
            //catch(Exception ex)
            //{
            //    _response.Success = Constants.ResponseFailure;
            //    _response.Message = ex.Message;
            //    return _response;
            //}
            try
            {
                if (model.PatientId == null)
                {
                    var patient = await _context.Patient.SingleOrDefaultAsync(x => x.Cnic == model.Cnic);

                    if (patient != null)
                    {
                        var appointmentCount = await _context.Appointment
                            .Where(a => a.PatientId == patient.PatientId &&
                                        a.DoctorId == model.DoctorId &&
                                        a.AppointmentDate.Date == model.AppoitmentTime.Date)
                            .CountAsync();

                        if (appointmentCount > 2)
                        {
                            return CreateErrorResponse("You can't take more than two appointments in a day from the same doctor.");
                        }
                    }

                    // Check for an existing appointment within 30 minutes of the selected time
                    if (await IsAppointmentTimeConflictAsync(model.DoctorId, model.AppoitmentTime))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 30 minutes after the existing appointment from the same doctor.");
                    }

                    // Create new patient, patient details, and appointment records
                    var patientObj = new PatientDoctor.domain.Entities.Patient
                    {
                        PatientId = Guid.NewGuid(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Status = 1,
                        Cnic = model.Cnic,
                        Gender = model.Gender,
                        DoctoerId = model.DoctorId,
                        DateofBirth = model.DateofBirth,
                    };
                    await _context.Patient.AddAsync(patientObj);

                    var patientDetails = new PatientDetails
                    {
                        PatiendDetailsId = Guid.NewGuid(),
                        PatientId = patientObj.PatientId,
                        PhoneNumber = model.PhoneNumber,
                        City = model.City,
                        BloodType = model.BloodType,
                        Status = 1,
                        MaritalStatus = model.MaritalStatus,
                        CheckUpStatus=0 // 0 for bydefault waiting, means patient is in waiting list
                    };
                    await _context.PatientDetails.AddAsync(patientDetails);
                    var patientAppointment = new Appointment
                    {
                        AppointmentId = Guid.NewGuid(),
                        DoctorId = model.DoctorId,
                        PatientId = patientObj.PatientId,
                        AppointmentDate = model.AppoitmentTime,
                        PatientDetailsId=patientDetails.PatiendDetailsId
                    };
                    await _context.Appointment.AddAsync(patientAppointment);

                    await _context.SaveChangesAsync();
                    //await transaction.CommitAsync();

                    return CreateSuccessResponse(Constants.DataSaved);
                }
                else
                {
                    // Handle editing an existing patient's appointment (similar logic as above)
                    var patient = await _context.Patient.FindAsync(model.PatientId);

                    if (patient == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Patient"));
                    }
                    var appointmentCount = await _context.Appointment
                       .Where(a => a.PatientId == patient.PatientId && a.DoctorId == patient.DoctoerId && a.AppointmentDate.Date == model.AppoitmentTime.Date)
                       .CountAsync();
                    if (appointmentCount > 2)
                    {
                        // Handle the case where the patient has already taken two appointments.
                        return CreateErrorResponse("You can't take more than two appointments in a day from the same doctor.");
                    }
                    // Check for an existing appointment within 30 minutes of the selected time
                    if (await IsAppointmentTimeConflictAsync(model.DoctorId, model.AppoitmentTime))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 30 minutes after the existing appointment from the same doctor.");
                    }
                    // Update existing patient and appointment records
                    patient.FirstName = model.FirstName;
                    patient.LastName = model.LastName;
                    patient.Cnic = model.Cnic;
                    patient.Gender = model.Gender;
                    patient.DoctoerId = model.DoctorId;
                    patient.DateofBirth = model.DateofBirth;

                    var patientDetails = await _context.PatientDetails.FindAsync(patient.PatientId);
                    if (patientDetails == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Patient Details"));
                    }

                    patientDetails.PhoneNumber = model.PhoneNumber;
                    patientDetails.City = model.City;
                    patientDetails.BloodType = model.BloodType;
                    patientDetails.MaritalStatus = model.MaritalStatus;

                    var existingAppointment = await _context.Appointment
                        .Where(x => x.PatientId == patient.PatientId && x.PatientDetailsId == patientDetails.PatiendDetailsId)
                        .FirstOrDefaultAsync();

                    if (existingAppointment == null)
                    {
                        return CreateErrorResponse(Constants.NotFound.Replace("{data}", "Appointment"));
                    }

                    existingAppointment.DoctorId = model.DoctorId;
                    existingAppointment.AppointmentDate = model.AppoitmentTime;

                    // Update tables
                    _context.Patient.Update(patient);
                    _context.PatientDetails.Update(patientDetails);
                    _context.Appointment.Update(existingAppointment);

                    await _context.SaveChangesAsync();
                    //await transaction.CommitAsync();

                    return CreateSuccessResponse(Constants.DataUpdate);

                }
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
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
    }
}
