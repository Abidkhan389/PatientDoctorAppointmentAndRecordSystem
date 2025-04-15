
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
using PatientDoctor.Application.Features.DoctorMedicine.Command;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Linq.Expressions;

namespace PatientDoctor.Infrastructure.Repositories.DoctorHoliday;
public class DoctorHolidayRepository(DocterPatiendDbContext _context, IResponse _response, ICountResponse _countResp, UserManager<ApplicationUser> _userManager) : IDoctorHolidayRepository
{
    public async Task<IResponse> ActiveInActive(ActiveInActiveDoctorHoliday model)
    {
       
        try
        {
            var doctorHoliDayObj = await _context.DoctorHolidays.FindAsync(model.Id);
            if (doctorHoliDayObj == null)
            {
                _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday");
                _response.Success = Constants.ResponseFailure;
            }
            doctorHoliDayObj.Status = model.Status;
            _context.DoctorHolidays.Update(doctorHoliDayObj);
            await _context.SaveChangesAsync();

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

    public async Task<IResponse> AddEditDoctorHoliday(AddEditDoctorHolidayCommand model)
    {
        try
        {
            DateTime today = DateTime.Today;
            DateTime filterToDate = model.ToDate.Date;
            DateTime filterFromDate = model.FromDate.Date;
            if (model.DoctorHolidayId is null) // Only check when adding a new holiday
            {
                bool CheckAlreadyDateExists = await _context.DoctorHolidays
                                        .AnyAsync(x =>
                                            x.DoctorId == model.DoctorId && // Ensure it checks only for the same doctor
                                            x.Status == 1 && // Check only active holidays
                                            (
                                                (filterFromDate >= x.FromDate.Date && filterFromDate <= x.ToDate.Date) || // New FromDate inside an existing range
                                                (filterToDate >= x.FromDate.Date && filterToDate <= x.ToDate.Date) || // New ToDate inside an existing range
                                                (x.FromDate.Date >= filterFromDate && x.FromDate.Date <= filterToDate) || // Existing FromDate inside new range
                                                (x.ToDate.Date >= filterFromDate && x.ToDate.Date <= filterToDate) // Existing ToDate inside new range
                                            )
                                        );

                if (CheckAlreadyDateExists)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday is already created on the same date");
                    _response.Success = Constants.ResponseFailure;
                    return _response;
                }
                var doctorholiday = new DoctorHolidays(model);
                await _context.DoctorHolidays.AddAsync(doctorholiday);
                await _context.SaveChangesAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataSaved;
            }
            else // Editing existing holiday
            {
                var existingHoliday = await _context.DoctorHolidays
                    .FirstOrDefaultAsync(x => x.DoctorHolidayId == model.DoctorHolidayId);

                if (existingHoliday == null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday not found");
                    _response.Success = Constants.ResponseFailure;
                    return _response;
                }

                // Check for date conflict excluding the current holiday
                bool CheckAlreadyDate = await _context.DoctorHolidays
                                    .AnyAsync(x =>
                                        x.DoctorHolidayId != model.DoctorHolidayId && // Ignore the current holiday being updated
                                        x.DoctorId == model.DoctorId && // Ensure we check only the same doctor's holidays
                                        x.Status == 1 && // Consider only active holidays
                                        (
                                            (filterFromDate >= x.FromDate.Date && filterFromDate <= x.ToDate.Date) || // New FromDate inside an existing range
                                            (filterToDate >= x.FromDate.Date && filterToDate <= x.ToDate.Date) || // New ToDate inside an existing range
                                            (x.FromDate.Date >= filterFromDate && x.FromDate.Date <= filterToDate) || // Existing FromDate inside new range
                                            (x.ToDate.Date >= filterFromDate && x.ToDate.Date <= filterToDate) // Existing ToDate inside new range
                                        )
                                    );

                if (CheckAlreadyDate)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday is conflicting with an existing holiday");
                    _response.Success = Constants.ResponseFailure;
                    return _response;
                }

                // Update existing holiday
                existingHoliday.FromDate = model.FromDate;
                existingHoliday.ToDate = model.ToDate;
                existingHoliday.Reason = model.Reason;

                await _context.SaveChangesAsync();

                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.Success = Constants.ResponseFailure;
        }
        return _response;
    }

    public async Task<IResponse> GetAllByProc(GetDoctorHolidayList model)
    {
        try
        {


            //DateTime today = DateTime.Today;
            //DateTime filterToDate = model.ToDate?.Date ?? today;
            //DateTime filterFromDate = model.FromDate?.Date ?? today;

            var userInfo = await _userManager.FindByIdAsync(model.LogedInDoctorId);
            var roleName = userInfo?.RoleName ?? string.Empty;

            model.Sort = string.IsNullOrWhiteSpace(model.Sort) ? "FirstName" : model.Sort;

            var data = (from doctorholiday in _context.DoctorHolidays
                        join main in _context.Users on doctorholiday.DoctorId equals main.Id
                        join user_details in _context.Userdetail on main.Id equals user_details.UserId
                        where (
                            (string.IsNullOrEmpty(model.FirstName) || user_details.FirstName.ToLower().Contains(model.FirstName.ToLower())) &&
                            (string.IsNullOrEmpty(model.LastName) || user_details.LastName.ToLower().Contains(model.LastName.ToLower())) &&
                             (model.FromDate == null || model.ToDate == null ||
        (doctorholiday.FromDate.Date <= model.ToDate.Value.Date &&
         doctorholiday.ToDate.Date >= model.FromDate.Value.Date)) &&
                            (roleName == "SuperAdmin" || roleName == "Receptionist" || main.Id == model.LogedInDoctorId)
                        )
                        select new VM_GetDoctorHolidayList
                        {
                            DoctorHolidayId = doctorholiday.DoctorHolidayId,
                            FirstName = user_details.FirstName,
                            LastName = user_details.LastName,
                            DoctorId = main.Id,
                            Status = doctorholiday.Status,
                            ToDate = doctorholiday.ToDate,
                            FromDate = doctorholiday.FromDate,
                            Reason = doctorholiday.Reason,
                        }).AsQueryable();



            var count = data.Count();
            var sorted = await HelperStatic.OrderBy(data, model.SortEx, model.OrderEx == "desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.DayName = Enumerable.Range(0, (item.ToDate - item.FromDate).Days + 1)
                               .Select(i => item.FromDate.AddDays(i).DayOfWeek.ToString())
                               .ToList();
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = _countResp;

        }
        catch(Exception ex)
        {
            _response.Message = ex.Message;
            _response.Success = Constants.ResponseFailure;

        }
        return _response;

    }

    public async Task<IResponse> GetByIdDoctorHoliday(GetByIdDoctorHoliday model)
    {
       var DoctorHolidayObj = await _context.DoctorHolidays
                                .Where(x => x.DoctorHolidayId == model.DoctorHolidayId)
                                .Select(y => new VM_GetByIdDoctorHoliday
                                {
                                    DoctorHolidayId = y.DoctorHolidayId,
                                    DoctorId = y.DoctorId,
                                    ToDate = y.ToDate, 
                                    FromDate = y.FromDate,
                                    Reason = y.Reason
                                })
                                .FirstOrDefaultAsync();

        if (DoctorHolidayObj is null)
        {
            _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday");
            _response.Success = Constants.ResponseFailure;
        }
        else
        {
            _response.Data = DoctorHolidayObj;
            _response.Message = Constants.GetData;
            _response.Success = Constants.ResponseSuccess;
        }
        return _response;
    }

    public async Task<IResponse> GetDoctorHolidayByDoctorIdForPatientAppointment(GetDoctorHolidayByDoctorIdForPatientAppointment model)
    {
        var DoctorHolidays = await _context.DoctorHolidays
                        .Where(x => x.DoctorId == model.DoctorId && x.Status == 1)
                        .Select(x => new VM_GetDoctorHolidayByDoctorIdForPatientAppointment
                        {
                           FromDate= x.FromDate.Date,
                           ToDate= x.ToDate.Date
                        })
                        .ToListAsync();
        if (DoctorHolidays is null)
        {
            _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday");
            _response.Success = Constants.ResponseFailure;
        }
        else
        {
            _response.Data = DoctorHolidays;
            _response.Message = Constants.GetData;
            _response.Success = Constants.ResponseSuccess;
        }
        return _response;   
    }
}
