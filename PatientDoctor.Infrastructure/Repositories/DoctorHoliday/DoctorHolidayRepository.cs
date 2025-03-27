
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;

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
                var CheckAlreadyDate = await _context.DoctorHolidays
                                         .Where(x =>
                                             (filterFromDate >= x.FromDate.Date && filterFromDate <= x.ToDate.Date) ||
                                             (filterToDate >= x.FromDate.Date && filterToDate <= x.ToDate.Date) ||
                                             (x.FromDate.Date >= filterFromDate && x.FromDate.Date <= filterToDate) ||
                                             (x.ToDate.Date >= filterFromDate && x.ToDate.Date <= filterToDate)
                                         ).FirstOrDefaultAsync();
                if (CheckAlreadyDate != null)
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
                var CheckAlreadyDate = await _context.DoctorHolidays
                    .Where(x => x.DoctorHolidayId != model.DoctorHolidayId) // Ignore the current Doctor
                    .Where(x =>
                        (filterFromDate >= x.FromDate.Date && filterFromDate <= x.ToDate.Date) ||
                        (filterToDate >= x.FromDate.Date && filterToDate <= x.ToDate.Date) ||
                        (x.FromDate.Date >= filterFromDate && x.FromDate.Date <= filterToDate) ||
                        (x.ToDate.Date >= filterFromDate && x.ToDate.Date <= filterToDate)
                    ).FirstOrDefaultAsync();

                if (CheckAlreadyDate != null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "Doctor Holiday is conflicting with an existing holiday");
                    _response.Success = Constants.ResponseFailure;
                    return _response;
                }

                // Update existing holiday
                existingHoliday.FromDate = model.FromDate;
                existingHoliday.ToDate = model.ToDate;
                existingHoliday.DayOfWeek = model.DayOfWeek;
                existingHoliday.Reason = model.Reason;
                existingHoliday.Status = model.Status;

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
            DateTime today = DateTime.Today;
            DateTime filterToDate = model.ToDate?.Date ?? today;
            DateTime filterFromDate = model.FromDate?.Date ?? today;

        var userInfo = await _userManager.FindByIdAsync(model.LogedInDoctorId);
        var roleName = userInfo?.RoleName ?? string.Empty;

        model.Sort = string.IsNullOrWhiteSpace(model.Sort) ? "FirstName" : model.Sort;

        var data = (from doctorholiday in _context.DoctorHolidays
                    join main in _context.Users on doctorholiday.DoctorId equals main.Id
                    join user_details in _context.Userdetail on main.Id equals user_details.UserId
                    where (
                        (string.IsNullOrEmpty(model.FirstName) || user_details.FirstName.ToLower().Contains(model.FirstName.ToLower())) &&
                        (string.IsNullOrEmpty(model.LastName) || user_details.LastName.ToLower().Contains(model.LastName.ToLower())) &&
                        ((model.DayOfWeek == null) || (doctorholiday.DayOfWeek == model.DayOfWeek)) &&
                        (model.FromDate == null || doctorholiday.FromDate.Date == filterFromDate) &&
                        (model.ToDate == null || (doctorholiday.ToDate.Date == filterToDate)) &&
                        (roleName == "SuperAdmin" || roleName == "Receptionist" || main.Id == model.LogedInDoctorId)
                    )
                    select new VM_GetDoctorHolidayList
                    {
                        FirstName = user_details.FirstName,
                        LastName = user_details.LastName,
                        DoctorId = main.Id,
                        Status = doctorholiday.Status,
                        ToDate=doctorholiday.ToDate,
                        FromDate=doctorholiday.FromDate,
                        Reason= doctorholiday.Reason,
                        DayOfWeek= doctorholiday.DayOfWeek
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

    public async Task<IResponse> GetByIdDoctorHoliday(GetByIdDoctorHoliday model)
    {
       var DoctorHolidayObj = await _context.DoctorHolidays
                                .Where(x => x.DoctorHolidayId == model.DoctorHolidayId)
                                .Select(y => new VM_GetByIdDoctorHoliday
                                {
                                    DoctorHolidayId = y.DoctorHolidayId,
                                    DoctorId = y.DoctorId,
                                    DayOfWeek = y.DayOfWeek, // Corrected: No need to parse
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
}
