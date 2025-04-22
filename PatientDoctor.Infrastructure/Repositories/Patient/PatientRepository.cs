using Microsoft.AspNetCore.Identity;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Application.Features.Patient.Quries;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor;
using PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf;
using PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using System.Globalization;
using Newtonsoft.Json;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Contracts.Persistance.ISmsRepository;
using PatientDoctor.Application.Helpers.AppointmentSms;

namespace PatientDoctor.Infrastructure.Repositories.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IPatientAppointmentSmsRepository _patientAppointmentSmsRepository;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountResponse _countResp;
        private readonly IConfiguration _configuration;
        private readonly ICryptoService _crypto;
        private readonly IPatientCheckUpHistroyRepository _patientCheckUpHistroy;

        public PatientRepository(DocterPatiendDbContext context, IPatientAppointmentSmsRepository patientAppointmentSmsRepository,
            IResponse response, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, ICountResponse countResp,
            IConfiguration configurations, ICryptoService crypto, IPatientCheckUpHistroyRepository patientCheckUpHistroy)
        {
            this._context = context;
            _patientAppointmentSmsRepository = patientAppointmentSmsRepository;
            this._response = response;
            this._userManager = userManager;
            this._countResp = countResp;
            this._configuration = configurations;
            this._crypto = crypto;
            _patientCheckUpHistroy = patientCheckUpHistroy;
        }
        private IResponse CreateSuccessResponse(string message)
        {
            return new Response { Success = Constants.ResponseSuccess, Message = message };
        }

        private IResponse CreateErrorResponse(string message)
        {
            return new Response { Success = Constants.ResponseFailure, Message = message };
        }

        private async Task<bool> IsAppointmentTimeConflictAsync(string doctorId, DateTime appointmentTime,string TimeSlot)
        {
            var minimumAllowedTime = appointmentTime.AddMinutes(Convert.ToInt32(_configuration["PatientSettings:PatientAllowedTime"]));
            var existingAppointment = await _context.Appointment
                .Where(x => x.DoctorId == doctorId &&
                            x.AppointmentDate.Date == appointmentTime.Date &&
                            x.TimeSlot == TimeSlot &&
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
                    if (await IsAppointmentTimeConflictAsync(model.AddEditPatientObj.DoctorId, model.AddEditPatientObj.AppoitmentDate, model.AddEditPatientObj.TimeSlot))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 10 minutes after the existing appointment from the same doctor.");
                    }
                    // Check if patient already exists
                    var existingPatient = await (
                        from p in _context.Patient
                        join pd in _context.PatientDetails on p.PatientId equals pd.PatientId
                        where
                            (
                                (p.FirstName.ToLower() == model.AddEditPatientObj.FirstName.ToLower() ||
                                 p.LastName.ToLower() == model.AddEditPatientObj.LastName.ToLower())
                                &&
                                p.Gender.ToLower() == model.AddEditPatientObj.Gender.ToLower()
                                &&
                                pd.PhoneNumber == model.AddEditPatientObj.PhoneNumber
                            )
                            ||
                            (
                                p.Cnic.ToLower() == model.AddEditPatientObj.Cnic.ToLower() &&
                                p.TrackingNumber == model.AddEditPatientObj.TrackingNumber
                            )
                        select new { Patient = p, PatientDetails = pd }
                    ).FirstOrDefaultAsync();



                    Guid patientId;
                    Guid patientDetailsId;
                    existingPatient = (existingPatient != null &&
                   existingPatient.Patient.FirstName.ToLower() == model.AddEditPatientObj.FirstName.ToLower() &&
                   existingPatient.Patient.LastName.ToLower() == model.AddEditPatientObj.LastName.ToLower())
                   ? existingPatient
                   : null;

                    if (existingPatient == null)
                    {
                        // Create Patient
                        var patient = new PatientDoctor.domain.Entities.Patient
                        {
                            PatientId = Guid.NewGuid(),
                            FirstName = model.AddEditPatientObj.FirstName,
                            LastName = model.AddEditPatientObj.LastName,
                            Status = 1,
                            Cnic = model.AddEditPatientObj.Cnic,
                            Gender = model.AddEditPatientObj.Gender,
                            DoctoerId = model.AddEditPatientObj.DoctorId,
                            Age = model.AddEditPatientObj.Age,
                            Description = ""
                        };
                        await _context.Patient.AddAsync(patient);

                        // Create Patient Details
                        var patientDetails = new PatientDetails
                        {
                            PatiendDetailsId = Guid.NewGuid(),
                            PatientId = patient.PatientId,
                            PhoneNumber = model.AddEditPatientObj.PhoneNumber,
                            City = model.AddEditPatientObj.City,
                            BloodType = model.AddEditPatientObj.BloodType,
                            Status = 1,
                            MaritalStatus = model.AddEditPatientObj.MaritalStatus,
                            CheckUpStatus = 0, // 0 = Waiting
                            CreatedBy = model.UserId,
                            CreatedOn = DateTime.Now
                        };
                        await _context.PatientDetails.AddAsync(patientDetails);

                        // Store IDs for appointment
                        patientId = patient.PatientId;
                        patientDetailsId = patientDetails.PatiendDetailsId;
                    }
                    else
                    {
                        // Use existing IDs
                        patientId = existingPatient.Patient.PatientId;
                        patientDetailsId = existingPatient.PatientDetails.PatiendDetailsId;
                    }

                    // Create Appointment (common code for both conditions)
                    var appointment = new Appointment
                    {
                        AppointmentId = Guid.NewGuid(),
                        DoctorId = model.AddEditPatientObj.DoctorId,
                        PatientId = patientId,
                        AppointmentDate = model.AddEditPatientObj.AppoitmentDate,
                        TimeSlot = model.AddEditPatientObj.TimeSlot,
                        PatientDetailsId = patientDetailsId,
                        DoctorFee=model.AddEditPatientObj.doctorFee,
                        PatientCheckUpDayId = model.AddEditPatientObj.PatientCheckUpDayId
                    };
                    await _context.Appointment.AddAsync(appointment);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var patientAppointmentSmsRequest = new PatientAppointmentSmsRequest
                    {
                        PatientMobileNumber = model.AddEditPatientObj.PhoneNumber,
                        DoctorName = await _context.Userdetail
                                    .Where(x => x.UserId == model.AddEditPatientObj.DoctorId)
                                    .Select(y => y.FirstName + " " + y.LastName)
                                    .FirstOrDefaultAsync(),
                        AppointmentDate= model.AddEditPatientObj.AppoitmentDate,
                        TimeSlot = model.AddEditPatientObj.TimeSlot,
                    };
                    _patientAppointmentSmsRepository.SendSmsAsync(patientAppointmentSmsRequest);
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
                       .Where(a => a.PatientId == patient.PatientId && a.DoctorId == patient.DoctoerId && a.AppointmentDate.Date == model.AddEditPatientObj.AppoitmentDate.Date)
                       .CountAsync();   
                    if (appointmentCount > 2)
                    {
                        // Handle the case where the patient has already taken two appointments.
                        return CreateErrorResponse("You can't take more than two appointments in a day from the same doctor.");
                    }
                    // Check for an existing appointment within 10 minutes of the selected time
                    if (await IsAppointmentTimeConflictAsync(model.AddEditPatientObj.DoctorId, model.AddEditPatientObj.AppoitmentDate, model.AddEditPatientObj.TimeSlot))
                    {
                        return CreateErrorResponse("Please choose an appointment time that is at least 10 minutes after the existing appointment from the same doctor.");
                    }
                    // Update existing patient and appointment records
                    patient.FirstName = model.AddEditPatientObj.FirstName;
                    patient.LastName = model.AddEditPatientObj.LastName;
                    patient.Cnic = model.AddEditPatientObj.Cnic;
                    patient.Gender = model.AddEditPatientObj.Gender;
                    patient.DoctoerId = model.AddEditPatientObj.DoctorId;
                    patient.Age = model.AddEditPatientObj.Age;

                    var patientDetails = await _context.PatientDetails.Where(x=>x.PatientId== patient.PatientId).FirstOrDefaultAsync();
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
                    existingAppointment.AppointmentDate = model.AddEditPatientObj.AppoitmentDate;
                    existingAppointment.TimeSlot = model.AddEditPatientObj.TimeSlot;
                    existingAppointment.PatientCheckUpDayId = model.AddEditPatientObj.PatientCheckUpDayId   ;

                    // Update tables
                    _context.Patient.Update(patient);
                    _context.PatientDetails.Update(patientDetails);
                    _context.Appointment.Update(existingAppointment);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var patientAppointmentSmsRequest = new PatientAppointmentSmsRequest
                    {
                        PatientMobileNumber = model.AddEditPatientObj.PhoneNumber,
                        DoctorName = await _context.Userdetail
                                   .Where(x => x.UserId == model.AddEditPatientObj.DoctorId)
                                   .Select(y => y.FirstName + " " + y.LastName)
                                   .FirstOrDefaultAsync(),
                        AppointmentDate = model.AddEditPatientObj.AppoitmentDate,
                        TimeSlot = model.AddEditPatientObj.TimeSlot,
                    };
                    _patientAppointmentSmsRepository.SendSmsAsync(patientAppointmentSmsRequest);
                    return CreateSuccessResponse(Constants.DataUpdate);

                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
                return CreateErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetAllByProc(GetPatientListWithUser model) 
        {
            var userInfo = await _userManager.FindByIdAsync(model.UserId);
            var roleName = userInfo?.RoleName;
            DateTime filterDate = model.getPatientListObj.appoitmentDate?.Date ?? DateTime.Today;
            model.Sort = model.Sort == null || model.Sort == "" ? "FirstName" : model.Sort;
            var data = (from patient in _context.Patient
                        join main in _context.Users on patient.DoctoerId equals main.Id
                        join p_details in _context.PatientDetails on patient.PatientId equals p_details.PatientId
                        join App in _context.Appointment on patient.PatientId equals App.PatientId
                        where (
                                (string.IsNullOrEmpty(model.getPatientListObj.FirstName) || patient.FirstName.ToLower().Contains(model.getPatientListObj.FirstName.ToLower()))
                                && (string.IsNullOrEmpty(model.getPatientListObj.LastName) || patient.LastName.ToLower().Contains(model.getPatientListObj.LastName.ToLower()))
                             && (string.IsNullOrEmpty(model.getPatientListObj.City) || p_details.City.ToLower().Contains(model.getPatientListObj.City.ToLower()))
                             && (string.IsNullOrEmpty(model.getPatientListObj.Cnic) || patient.Cnic.ToLower().Contains(model.getPatientListObj.Cnic))
                             && (string.IsNullOrEmpty(model.getPatientListObj.MobileNumber) || p_details.PhoneNumber.ToLower().Contains(model.getPatientListObj.MobileNumber.ToLower()))
                              && App.AppointmentDate.Date == filterDate.Date
                              && (roleName == "SuperAdmin" || roleName == "Receptionist" || patient.DoctoerId == model.UserId)
                              )
                        select new VM_Patient
                        {
                            PatientId = patient.PatientId,
                            FirstName = patient.FirstName,
                            LastName=patient.LastName,
                            Gender = patient.Gender,
                            DoctorName = main.UserName,
                            DoctorId = main .Id,
                            AppointmentTime = App.AppointmentDate.Date,
                            PatientPhoneNumber = p_details.PhoneNumber,
                            DoctorPhoneNumber = main.PhoneNumber,
                            City = p_details.City,
                            BloodType = p_details.BloodType,
                            Cnic = patient.Cnic,
                            Status =patient.Status,
                            TimeSlot = App.TimeSlot,
                            MaritalStatus = p_details.MaritalStatus,
                            CheckUpStatus = Convert.ToInt16(App.CheckUpStatus),
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
                                        DoctorName = main.UserName,
                                        PhoneNumber = p_Details.PhoneNumber,
                                        DoctorFee = app.DoctorFee ,
                                        FirstName = patient.FirstName,
                                        LastName = patient.LastName,
                                        City = p_Details.City,
                                        Cnic = patient.Cnic,
                                        BloodType = p_Details.BloodType,
                                        MaritalStatus = p_Details.MaritalStatus,
                                        Gender = patient.Gender,
                                        Age = patient.Age,
                                        AppoitmentDate = app.AppointmentDate,
                                        PatientCheckUpDayId = app.PatientCheckUpDayId,
                                        TimeSlot = app.TimeSlot
                                    }).FirstOrDefaultAsync();
           
            if (patientobj != null)
            {
                var gettimeslotsObj = new GetDoctorTimeSlotsByDayIdAndDoctorId
                {
                    DoctorId = patientobj.DoctorId,
                    DayId = patientobj.PatientCheckUpDayId ?? 0,
                    AppointmentDate = patientobj.AppoitmentDate
                };

                var DoctorAvailabalTimeSlot = await GetDoctorAppointmentsSlotsOfDay(gettimeslotsObj);
                if (DoctorAvailabalTimeSlot != null)
                {
                    patientobj.vM_DoctorTimeSlotsPerDay = (VM_DoctorTimeSlotsPerDay?)DoctorAvailabalTimeSlot.Data;
                }
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

        public async Task<IResponse> AddEditPatientDescription(AddPatientDescriptionCommand model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (model.Id == null)
                {


                    var patientDetails = await _context.PatientDetails.FirstOrDefaultAsync(pd => pd.PatientId == model.PatientId);
                    var patient = await _context.Patient.FirstOrDefaultAsync(pd => pd.PatientId == model.PatientId);
                    var appointmentDetail = await _context.Appointment.FirstOrDefaultAsync(pd => pd.PatientId == model.PatientId);

                    if (patientDetails != null)
                    {

                        var patienCheckUpDescription = new Prescription(model);
                        if (patient?.TrackingNumber is null)
                        {
                            var trackPatientNumber = await _patientCheckUpHistroy.FetchPatientTrackingNumberByPatientId(patient.PatientId);

                            if (trackPatientNumber.Success) // Check if response is successful
                            {
                                patient.TrackingNumber = trackPatientNumber.Data as string; // Safe casting to string
                            }
                        }
                        //patientDetails.CheckUpStatus = 1; // update check status to 1, its means patient is checked
                        appointmentDetail.CheckUpStatus = true;
                        patientDetails.CreatedOn = DateTime.Now;
                        await _context.Prescriptions.AddAsync(patienCheckUpDescription);
                        _context.PatientDetails.Update(patientDetails);
                        _context.Appointment.Update(appointmentDetail);
                        _context.Patient.Update(patient);
                        //var data = await (from patnt in _context.Patient
                        //                  join p_details in _context.PatientDetails on patnt.PatientId equals p_details.PatientId
                        //                  join main in _context.Users on patnt.DoctoerId equals main.Id
                        //                  join DctrCheckUpFeeDetls in _context.DoctorCheckUpFeeDetails on patient.DoctoerId equals DctrCheckUpFeeDetls.DoctorId
                        //                  select new PatientCheckedUpFeeHistroyDto
                        //                  {
                        //                      DoctorId = patient.DoctoerId,
                        //                      DoctorName = main.UserName,
                        //                      DoctorEmail = main.Email,
                        //                      DoctorNumber = main.PhoneNumber,
                        //                      PatientId = patient.PatientId,
                        //                      PatientName = patient.FirstName + patient.LastName,
                        //                      PatientNumber = p_details.PhoneNumber,
                        //                      PatientCnic = patient.Cnic,
                        //                      CheckUpFee = DctrCheckUpFeeDetls.DoctorFee
                        //                  }).FirstOrDefaultAsync();
                        //var patientCheckedUpFeeHistroy = new PatientCheckedUpFeeHistroy(data);
                        //await _context.PatientCheckedUpFeeHistroy.AddAsync(patientCheckedUpFeeHistroy);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataSaved;
                    }
                }
                else
                {
                    var appointmentDetail = await _context.Appointment.FirstOrDefaultAsync(pd => pd.PatientId == model.PatientId);
                    var existingPrescription = await _context.Prescriptions
                    .Include(x => x.Medicines)
                    .FirstOrDefaultAsync(x => x.PrescriptionId == model.Id);

                    if (existingPrescription != null)
                    {
                        // Update prescription fields
                        existingPrescription.LeftVision = model.LeftVision;
                        existingPrescription.RightVision = model.RightVision;
                        existingPrescription.LeftMG = model.LeftMG;
                        existingPrescription.RightMG = model.RightMG;
                        existingPrescription.LeftEOM = model.LeftEOM;
                        existingPrescription.RightEOM = model.RightEom;
                        existingPrescription.LeftOrtho = model.LeftOrtho;
                        existingPrescription.RightOrtho = model.RightOrtho;
                        existingPrescription.LeftTension = model.LeftTension;
                        existingPrescription.RightTension = model.RightTension;
                        existingPrescription.LeftAntSegment = model.LeftAntSegment;
                        existingPrescription.RightAntSegment = model.RightAntSegment;
                        existingPrescription.LeftDisc = model.LeftDisc;
                        existingPrescription.RightDisc = model.RightDisc;
                        existingPrescription.LeftMacula = model.LeftMacula;
                        existingPrescription.RightMacula = model.RightMacula;
                        existingPrescription.LeftPeriphery = model.LeftPeriphery;
                        existingPrescription.RightPeriphery = model.RightPeriphery;
                        existingPrescription.Complaint = model.ComplaintOf;
                        existingPrescription.Diagnosis = model.Diagnosis;
                        existingPrescription.Plan = model.Plan;
                        existingPrescription.UpdatedOn = DateTime.UtcNow;
                        existingPrescription.UpdatedBy = model.UserId;

                        //// 1. Remove old medicines
                        _context.PrescriptionMedicines.RemoveRange(existingPrescription.Medicines);

                        // 2. Add new medicines
                        var updatedMedicines = model.medicine.Select(m => new PrescriptionMedicine
                        {
                            PrescriptionId = existingPrescription.PrescriptionId,
                            MedicineId = m.MedicineId,
                            PotencyId=m.PotencyId,
                            DurationInDays = m.DurationInDays,
                            Morning = m.Morning,
                            Afternoon = m.Afternoon,
                            Evening = m.Evening,
                            Night = m.Night,
                            RepeatEveryHours = m.RepeatEveryHours,
                            RepeatEveryTwoHours = m.RepeatEveryTwoHours
                        }).ToList();

                        existingPrescription.Medicines = updatedMedicines;

                        _context.Prescriptions.Update(existingPrescription); //its optionall
                        appointmentDetail.CheckUpStatus = true;
                        _context.Appointment.Update(appointmentDetail);
                        // Final Save
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    _response.Success = Constants.ResponseSuccess;
                    _response.Message = Constants.DataUpdate;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
                _response.Success = Constants.ResponseFailure;
                _response.Message = ex.Message;
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
                               (string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.PatientName)
                                    || patient.FirstName.ToLower().Contains(model.GetPatientAppoitmentsListObj.PatientName.ToLower()))
                             && (string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.City)
                                    || p_details.City.ToLower().Contains(model.GetPatientAppoitmentsListObj.City.ToLower()))
                             && (string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.Cnic)
                                    || patient.Cnic.ToLower().Contains(model.GetPatientAppoitmentsListObj.Cnic.ToLower()))
                             && (string.IsNullOrEmpty(model.GetPatientAppoitmentsListObj.MobileNumber)
                                    || p_details.PhoneNumber.ToLower().Contains(model.GetPatientAppoitmentsListObj.MobileNumber.ToLower()))
                             && (App.DoctorId == model.DocterId
                                    && App.PatientId == patient.PatientId
                                    && p_details.PatiendDetailsId == App.PatientDetailsId
                                    && App.AppointmentDate.Date == model.GetPatientAppoitmentsListObj.Todeydatetime.Date)
                             )
                        select new VM_Patient
                        {
                            PatientId = patient.PatientId,
                            FirstName = patient.FirstName ,
                            LastName=patient.LastName,
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
                var patientDescription= JsonConvert.DeserializeObject<AddPatientDescriptionCommand>(patientDescriptionEntity.Description);
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

        public async Task<IResponse> GetPatientsRecordWithDoctorProc(GetPatientRecordListWithDoctor model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "DoctorName" : model.Sort;
            var data = (from patient in _context.Patient
                        join main in _context.Users on patient.DoctoerId equals main.Id
                        join p_details in _context.PatientDetails on patient.PatientId equals p_details.PatientId
                        join DctrCheckUpFeeDetls in _context.DoctorCheckUpFeeDetails on patient.DoctoerId equals DctrCheckUpFeeDetls.DoctorId
                        where (
                               (string.IsNullOrEmpty(model.getPatientRecordList.PatientName)
                                    || patient.FirstName.ToLower().Contains(model.getPatientRecordList.PatientName.ToLower()))
                             && (string.IsNullOrEmpty(model.getPatientRecordList.PatientName)
                                    || patient.LastName.ToLower().Contains(model.getPatientRecordList.PatientName.ToLower()))
                             && (string.IsNullOrEmpty(model.getPatientRecordList.PatientCnic)
                                    || patient.Cnic.ToLower().Contains(model.getPatientRecordList.PatientCnic.ToLower()))
                             && (string.IsNullOrEmpty(model.getPatientRecordList.DoctorName)
                                    || main.UserName.ToLower().Contains(model.getPatientRecordList.DoctorName.ToLower()))
                             && ((model.getPatientRecordList.PatientCheckUpDateFrom == null
                                    || model.getPatientRecordList.PatientCheckUpDateFrom <= DctrCheckUpFeeDetls.CreatedOn)
                             && (model.getPatientRecordList.PatientCheckUpDateTo == null
                                    || model.getPatientRecordList.PatientCheckUpDateTo >= p_details.CreatedOn))
                             && p_details.CheckUpStatus == 1
                            )
                        select new VM_PatientRecordListWithDoctor
                        {
                            PatientId = patient.PatientId,
                            PatientName = patient.FirstName + " " + patient.LastName,
                            PatientCnic = patient.Cnic,
                            PatientCheckUpDate = p_details.CreatedOn,
                            PatientCheckUpDoctorFee = DctrCheckUpFeeDetls.DoctorFee,
                            DoctorName = main.UserName,
                            DoctorEmail = main.Email,
                            DoctorId = patient.DoctoerId
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
        public async Task<IResponse> GetPatientDetailsForPdf(GetPatientDetailsForPdfRequest model)
        {
            var PatientDetailsForPdf = await (
                                            from patient in _context.Patient
                                            join p_details in _context.PatientDetails on patient.PatientId equals p_details.PatientId
                                            join main in _userManager.Users on patient.DoctoerId equals main.Id
                                            join DctrCheckUpFeeDetls in _context.DoctorCheckUpFeeDetails on patient.DoctoerId equals DctrCheckUpFeeDetls.DoctorId
                                            where (
                                                    patient.PatientId == model.PatientId
                                                    && main.Id == model.DoctorId
                                                 )
                                            select new VM_GetPatientDetailForPdf
                                            {
                                                PatientName= patient.FirstName+ patient.LastName,
                                                City =p_details.City,
                                                PatientMobileNumber=p_details.PhoneNumber,
                                                PatientCheckUpDate= (DateTime)p_details.CreatedOn,
                                                PatientCnic= patient.Cnic,
                                                PatientCheckUpDoctorFee= DctrCheckUpFeeDetls.DoctorFee,
                                                DoctorName = main.UserName,
                                                DoctorEmail= main.Email,
                                            }).FirstOrDefaultAsync();
            if (PatientDetailsForPdf != null)
            {
                _response.Data = PatientDetailsForPdf;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Patient");
            }
            return _response;
        }
        
        public async Task<IResponse> GetDoctorAppointmentsSlotsOfDay(GetDoctorTimeSlotsByDayIdAndDoctorId model)
            {
            // 1️⃣ Get the start and end dates for the selected month
            DateTime monthStart = new DateTime(model.AppointmentDate.Year, model.AppointmentDate.Month, 1);
            DateTime monthEnd = new DateTime(model.AppointmentDate.Year, model.AppointmentDate.Month, DateTime.DaysInMonth(model.AppointmentDate.Year, model.AppointmentDate.Month));

            // 2️⃣ Fetch holidays that INCLUDE the appointment date
            var holidays = await _context.DoctorHolidays
                .Where(x => x.DoctorId == model.DoctorId &&
                            x.Status == 1 && // Only active holidays
                            x.FromDate.Date <= model.AppointmentDate.Date &&  // ✅ Holiday must start before or on appointment date
                            x.ToDate.Date >= model.AppointmentDate.Date)      // ✅ Holiday must end on or after appointment date
                .ToListAsync();

            // 3️⃣ Generate list of holiday dates only if the appointment falls within a holiday
            List<string> holidayDates = new List<string>();

            foreach (var holiday in holidays)
            {
                DateTime current = model.AppointmentDate.Date; // ✅ Start from the appointment date

                while (current <= holiday.ToDate.Date && current <= monthEnd)
                {
                    holidayDates.Add(current.ToString("dd")); // Store only the day
                    current = current.AddDays(1); // Move to next day
                }
            }

            // 4️⃣ Generate response message only if holidays exist for the appointment date
            if (holidayDates.Any())
            {
                var message = "Doctor is on leave on date: " + string.Join(", ", holidayDates);
                if (holidayDates.Count > 1)
                {
                    message = "Doctor is on leave from dates: " + string.Join(", ", holidayDates);
                }

                return new Response
                {
                    Success = Constants.ResponseFailure,
                    Message = message
                };
            }



            var doctorObj = await _context.DoctorAvailabilities
                .Where(x => x.DoctorId == model.DoctorId && x.DayId == model.DayId && x.Status==1)
                .FirstOrDefaultAsync();

            if (doctorObj == null || string.IsNullOrEmpty(doctorObj.TimeSlotsJson))
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NoSlotAvaibale;
            }
            else
            {
                // Deserialize time slots
                var timeSlots = JsonConvert.DeserializeObject<List<DoctorTimeSlot>>(doctorObj.TimeSlotsJson);

                var doctorTimeSlotsPerDay = new VM_DoctorTimeSlotsPerDay
                {
                    DayId = model.DayId,
                    DoctorId = model.DoctorId,
                    DoctorSlots = new List<DoctorTimeSlots>()
                };

                foreach (var slot in timeSlots)
                {
                    var slotChunks = GenerateTimeChunks(slot.StartTime, slot.EndTime,doctorObj.AppointmentDurationMinutes);
                    doctorTimeSlotsPerDay.DoctorSlots.AddRange(slotChunks);
                }
                var appointmentObj = await _context.Appointment.Where(x => x.AppointmentDate.Date == model.AppointmentDate.Date &&
                                    x.DoctorId == model.DoctorId).Select(x=> x.TimeSlot).ToListAsync();
                doctorTimeSlotsPerDay.DoctorSlots.RemoveAll(slot => appointmentObj.Contains(slot.DoctorTime));

                _response.Data = doctorTimeSlotsPerDay;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
            }
            return _response;
        }

        /// <summary>
        /// Generates 15-minute chunks between the given start and end times.
        /// </summary>
        private static List<DoctorTimeSlots> GenerateTimeChunks(string startTime, string endTime, int AppointmentDurationMinutes)
        {
            var result = new List<DoctorTimeSlots>();

            if (!TimeSpan.TryParse(startTime, out TimeSpan start) || !TimeSpan.TryParse(endTime, out TimeSpan end))
            {
                return result; // Return empty if parsing fails
            }

            while (start <= end)
            {
                result.Add(new DoctorTimeSlots
                {
                    DoctorTime = ConvertTo12HourFormat(start.ToString(@"hh\:mm"))
                });

                start = start.Add(TimeSpan.FromMinutes(AppointmentDurationMinutes));
            }

            return result;
        }

        /// <summary>
        /// Converts 24-hour time format to 12-hour AM/PM format.
        /// </summary>
        private static string ConvertTo12HourFormat(string time)
        {
            return TimeSpan.TryParse(time, out TimeSpan parsedTime)
                ? DateTime.Today.Add(parsedTime).ToString("hh:mm tt", CultureInfo.InvariantCulture)
                : time;
        }

    }
}
