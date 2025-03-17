using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.Commands.ActiveInActive;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;

namespace PatientDoctor.Infrastructure.Repositories.PatientCheckUpHistroy;
public class PatientCheckUpHistroyRepository(DocterPatiendDbContext _context, UserManager<ApplicationUser> _userManager,
    ICountResponse _countResp, IConfiguration _configuration, IResponse _response) : IPatientCheckUpHistroyRepository
{
    public async Task<IResponse> GetAllByProc(GetAllPatientCheckUpHistroyByDoctor model)
    {
        
        model.Sort = model.Sort == null || model.Sort == "" ? "FirstName" : model.Sort;
        DateTime filterDate = model.getPatientHistoryListObj.AppoitmentDate?.Date ?? DateTime.Today;

        var data = (from main in _userManager.Users
                    join per in _context.Prescriptions on main.Id equals per.DoctorId
                    join userDetails in _context.Userdetail on main.Id equals userDetails.UserId
                    join patient in _context.Patient on per.PatientId equals patient.PatientId
                    join patientDetails in _context.PatientDetails on patient.PatientId equals patientDetails.PatientId
                    where (
                    //(string.IsNullOrEmpty(model.DoctorId) || per.DoctorId == model.DoctorId))
                      (string.IsNullOrEmpty(model.getPatientHistoryListObj.Cnic) || patient.Cnic.ToLower().Contains(model.getPatientHistoryListObj.Cnic))
                     && (string.IsNullOrEmpty(model.getPatientHistoryListObj.City) || patientDetails.City.ToLower().Contains(model.getPatientHistoryListObj.City))
                     && (string.IsNullOrEmpty(model.getPatientHistoryListObj.Plan) || per.Plan.ToLower().Contains(model.getPatientHistoryListObj.Plan))
                     && (string.IsNullOrEmpty(model.getPatientHistoryListObj.FirstName) || patient.FirstName.ToLower().Contains(model.getPatientHistoryListObj.FirstName))
                     && (string.IsNullOrEmpty(model.getPatientHistoryListObj.LastName) || patient.LastName.ToLower().Contains(model.getPatientHistoryListObj.LastName))
                     && (string.IsNullOrEmpty(model.getPatientHistoryListObj.PhoneNumber) || patientDetails.PhoneNumber.ToLower().Contains(model.getPatientHistoryListObj.PhoneNumber))
                     &&((model.getPatientHistoryListObj.AppoitmentDate == null) ||(filterDate.Date == per.CreatedAt.Date))
                     //&& (main.Id == model.LogedInDoctorId)
                     )

                    select new VM_PatientCheckUpHistroyList
                    {
                        PrescriptionId = per.PrescriptionId,
                        DoctorId = per.DoctorId,
                        DoctorName = userDetails.FirstName + " " + userDetails.LastName,
                        PatientId = per.PatientId,
                        FirstName = patient.FirstName,
                        LastName=patient.LastName,
                        patientCnic = patient.Cnic,
                        patientCity = patientDetails.City,
                        PatientPhoneNumber = patientDetails.PhoneNumber,
                        Status = per.Status,
                        AppointmentDate = per.CreatedAt.Date
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

    public async Task<IResponse> GetPatientCheckHistroyById(GetPatientCheckHistroyById model)
    {
        var result = await _context.Prescriptions
                .Where(x => x.PrescriptionId == model.Id)
                .Include(x => x.Medicines)
                .Select(x => new VM_PatientCheckHistroyById
                {
                    PrescriptionId = x.PrescriptionId,
                    PatientId = x.PatientId,
                    DoctorId = x.DoctorId,
                    // Eye Examination Details
                    LeftVision = x.LeftVision,
                    RightVision = x.RightVision,
                    LeftMG = x.LeftMG,
                    RightMG = x.RightMG,
                    LeftEOM = x.LeftEOM,
                    RightEOM = x.RightEOM,
                    LeftOrtho = x.LeftOrtho,
                    RightOrtho = x.RightOrtho,
                    LeftTension = x.LeftTension,
                    RightTension = x.RightTension,
                    LeftAntSegment = x.LeftAntSegment,
                    RightAntSegment = x.RightAntSegment,
                    LeftDisc = x.LeftDisc,
                    RightDisc = x.RightDisc,
                    LeftMacula = x.LeftMacula,
                    RightMacula = x.RightMacula,
                    LeftPeriphery = x.LeftPeriphery,
                    RightPeriphery = x.RightPeriphery,

                    Status = x.Status,

                    // Other Details
                    Complaint = x.Complaint,
                    Diagnosis = x.Diagnosis,
                    Plan = x.Plan,
                    CreatedAt = x.CreatedAt,

                    // Medicines
                    Medicine = x.Medicines.Select(m => new VM_PrescriptionMedicine
                    {
                        Id = m.Id,
                        MedicineId = m.MedicineId,
                        Morning = m.Morning,
                        Afternoon = m.Afternoon,
                        Evening = m.Evening,
                        Night = m.Night,
                        RepeatEveryHours = m.RepeatEveryHours,
                        RepeatEveryTwoHours = m.RepeatEveryTwoHours,
                        DurationInDays = m.DurationInDays
                    }).ToList()
                })
                .FirstOrDefaultAsync();

        if (result != null)
        {
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.DataUpdate;
            _response.Data = result;
        }
        else
        {
            _response.Success = Constants.ResponseFailure;
            _response.Message = Constants.NotFound;
        }
        return _response;
    }
    public async Task<IResponse> ActiveInActive(ActiveInActivePatientCheckUpHistory model)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var patientCheckUpDescription = await _context.Prescriptions.FindAsync(model.Id);
            if (patientCheckUpDescription == null)
            {
                _response.Message = Constants.NotFound.Replace("{data}", "patientCheckUpDescription");
                _response.Success = Constants.ResponseFailure;
            }
            patientCheckUpDescription.Status = model.Status;
            _context.Prescriptions.Update(patientCheckUpDescription);
            var patient = await _context.Patient.Where(x => x.PatientId == patientCheckUpDescription.PatientId).FirstOrDefaultAsync();
            var patiendetial = await _context.PatientDetails.Where(x => x.PatientId == patient.PatientId).FirstOrDefaultAsync();
            patient.Status = model.Status;
            patiendetial.Status = model.Status;
            _context.Patient.Update(patient);
            _context.PatientDetails.Update(patiendetial);
            await _context.SaveChangesAsync();
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
}

