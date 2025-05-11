using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
using PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Globalization;

namespace PatientDoctor.Infrastructure.Repositories.DoctorAvailability;
public class DoctorAvailabilityRepository(DocterPatiendDbContext _context, IResponse _response, ICountResponse _countResp,
                                        UserManager<ApplicationUser> _userManager) : IDoctorAvailabilityRepository

{
    public async Task<IResponse> AddEditDoctorAvaibality(AddEditDoctorAvailabilityWithUserId model)
    {
        try
        {
            var doctorId = model.AddEditDoctorAvailabilityObj.DoctorId;
            var dayIds = model.AddEditDoctorAvailabilityObj.DayIds;
            var doctorTimeSlots = model.AddEditDoctorAvailabilityObj.DoctorTimeSlots
                .Select(slot => new DoctorTimeSlot
                {
                    StartTime = slot.StartTime, // Already in 24-hour format
                    EndTime = slot.EndTime
                }).ToList();
            List<DoctorAvailabilities> DoctorAvailabilitiesList = new List<DoctorAvailabilities>();
            if (model.AddEditDoctorAvailabilityObj.Id == null)
            {
                // **Fetch existing availabilities for the given doctor and day IDs**
                var existingAvailabilities = await _context.DoctorAvailabilities
                    .Where(x => x.DoctorId == doctorId && dayIds.Contains(x.DayId))
                    .ToListAsync();

                foreach (var dayId in dayIds)
                {
                    var existingAvailability = existingAvailabilities.FirstOrDefault(x => x.DayId == dayId);

                    if (existingAvailability != null)
                    {
                        // **Update existing record**
                        existingAvailability.TimeSlots = doctorTimeSlots;
                        existingAvailability.TimeSlotsJson = JsonConvert.SerializeObject(doctorTimeSlots);
                        existingAvailability.CreatedAt = DateTime.Now;
                        existingAvailability.CreatedOn = DateTime.Now;
                        existingAvailability.CreatedBy = model.UserId;
                        _context.DoctorAvailabilities.Update(existingAvailability);
                    }
                    else
                    {
                        // **Insert new record**
                        var newAvailability = new DoctorAvailabilities
                        {
                            AvailabilityId = Guid.NewGuid(),
                            DoctorId = doctorId,
                            DayId = dayId,
                            TimeSlots = doctorTimeSlots,
                            TimeSlotsJson = JsonConvert.SerializeObject(doctorTimeSlots),
                            AppointmentDurationMinutes = model.AddEditDoctorAvailabilityObj.AppointmentDurationMinutes,
                            CreatedAt = DateTime.Now,
                            Status = 1,
                            CreatedOn = DateTime.Now,
                            CreatedBy = model.UserId
                        };

                        DoctorAvailabilitiesList.Add(newAvailability);
                    }
                }
                await _context.DoctorAvailabilities.AddRangeAsync(DoctorAvailabilitiesList);
                await _context.SaveChangesAsync();

                _response.Message = Constants.DataSaved;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                var existingObj = await _context.DoctorAvailabilities
    .Where(x => x.AvailabilityId == model.AddEditDoctorAvailabilityObj.Id)
    .FirstOrDefaultAsync();

                if (existingObj == null)
                {
                    _response.Success = Constants.ResponseFailure;
                    _response.Message = Constants.NotFound.Replace("{data}", "DoctorAvailabilities");
                }
                else
                {
                    existingObj.DoctorId = model.AddEditDoctorAvailabilityObj.DoctorId;
                    existingObj.DayId = model.AddEditDoctorAvailabilityObj.DayIds.FirstOrDefault(); // ✅ Directly assign it, no need for FirstOrDefault()
                    existingObj.TimeSlots = doctorTimeSlots;
                    existingObj.TimeSlotsJson = JsonConvert.SerializeObject(doctorTimeSlots);
                    existingObj.AppointmentDurationMinutes = model.AddEditDoctorAvailabilityObj.AppointmentDurationMinutes;
                    existingObj.UpdatedOn = DateTime.Now;
                    existingObj.UpdatedBy = model.UserId;

                    _context.DoctorAvailabilities.Update(existingObj);
                    await _context.SaveChangesAsync();

                    _response.Message = "Doctor availability updated successfully.";
                    _response.Success = Constants.ResponseSuccess;
                }


            }
            return _response;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.Success = Constants.ResponseFailure;
            return _response;
        }

    }
    private static string ConvertTo24HourFormat(string time)
    {
        return DateTime.ParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay.ToString(@"hh\:mm");
    }
    private static string ConvertTo12HourFormat(string time)
    {
        return TimeSpan.TryParse(time, out TimeSpan parsedTime)
            ? DateTime.Today.Add(parsedTime).ToString("hh:mm tt", CultureInfo.InvariantCulture) // Convert TimeSpan to 12-hour format
            : time; // Return original if parsing fails
    }

    public async Task<IResponse> GetByIdDoctorAvaibality(Guid Id)
    {
        var availability = await _context.DoctorAvailabilities
            .Include(a => a.Doctor) // Include doctor details if needed
            .FirstOrDefaultAsync(a => a.AvailabilityId == Id);

        if (availability == null)
        {
            _response.Success = Constants.ResponseFailure;
            _response.Message = Constants.NotFound.Replace("{data}", "DoctorAvailabities");
        }
        else
        {
            var timeSlots = string.IsNullOrEmpty(availability.TimeSlotsJson)
                ? new List<DoctorTimeSlot>()
                : JsonConvert.DeserializeObject<List<DoctorTimeSlot>>(availability.TimeSlotsJson)
                    .Select(slot => new DoctorTimeSlot
                    {
                        StartTime = slot.StartTime, 
                        EndTime = slot.EndTime
                    })
                    .ToList();

            var result = new VM_DoctorAvailabilites
            {
                AvailabilityId = availability.AvailabilityId,
                DoctorId = availability.DoctorId,
                DoctorName = availability.Doctor?.UserName, // Ensure doctor is included
                DayId = availability.DayId,
                DayName = Enum.GetName(typeof(DayOfWeek), availability.DayId), // Convert int to enum name
                DoctorTimeSlots = timeSlots,
                AppointmentDurationMinutes = availability.AppointmentDurationMinutes,
                CreatedAt = availability.CreatedAt
            };

            _response.Data = result;
            _response.Message = Constants.GetData;
            _response.Success = Constants.ResponseSuccess;
        }

        return _response;
    }
    public async Task<IResponse> GetAllByProc(GetDoctorAvailabiltiesList model)
    {
        model.Sort = string.IsNullOrEmpty(model.Sort) ? "DayId" : model.Sort;
        try
        {
            var userInfo = await _userManager.FindByIdAsync(model.UserId);
            var roleName = userInfo?.RoleName;
            var data = (from availability in _context.DoctorAvailabilities
                        join doctor in _context.Users on availability.DoctorId equals doctor.Id
                        where (
                        (model.DayId == null || availability.DayId == model.DayId)
                         && (roleName == "SuperAdmin" || roleName == "Receptionist" || availability.DoctorId == model.UserId)
                        )
                        select new
                        {
                            AvailabilityId = availability.AvailabilityId,
                            DoctorId = availability.DoctorId,
                            DoctorName = doctor.UserName,
                            DayId = availability.DayId,
                            TimeSlotsJson = availability.TimeSlotsJson,
                            AppointmentDurationMinutes = availability.AppointmentDurationMinutes,
                            CreatedAt = availability.CreatedAt,
                            Status = availability.Status
                        })
            .AsEnumerable() // Convert query to IEnumerable (Client-side execution starts)
            .Select(avail => new VM_DoctorAvailabilites
            {
                AvailabilityId = avail.AvailabilityId,
                DoctorId = avail.DoctorId,
                DoctorName = avail.DoctorName,
                DayId = avail.DayId,
                DayName = Enum.GetName(typeof(DayOfWeek), avail.DayId), // Execute on client-side
                DoctorTimeSlots = string.IsNullOrEmpty(avail.TimeSlotsJson)
                    ? new List<DoctorTimeSlot>()
                    : JsonConvert.DeserializeObject<List<DoctorTimeSlot>>(avail.TimeSlotsJson),
                AppointmentDurationMinutes = avail.AppointmentDurationMinutes,
                CreatedAt = avail.CreatedAt,
                Status = avail.Status
            })
            .ToList(); // Use ToList() instead of ToListAsync()

            var count = data.Count();
            var sorted = HelperStatic.OrderBy(data.AsQueryable(), model.SortEx, model.OrderEx == "desc")
                                     .Skip(model.Start)
                                     .Take(model.LimitEx)
                                     .ToList(); // Use ToList() instead of ToListAsync()

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
        catch (Exception ex)
        {
            _response.Success = Constants.ResponseFailure;
            _response.Message = ex.Message;
            _countResp.TotalCount =  0;
            return _response;
        }
    }



    public async Task<IResponse> ActiveInActive(ActiveInActiveDoctorAvailability model)
    {
        try
        {
            var doctoravailabilitiesObj = await _context.DoctorAvailabilities.FindAsync(model.Id);
            if (doctoravailabilitiesObj == null)
            {
                _response.Message = Constants.NotFound.Replace("{data}", "DoctorAvailabities");
                _response.Success = Constants.ResponseFailure;
            }
            doctoravailabilitiesObj.Status = model.Status;
             _context.DoctorAvailabilities.Update(doctoravailabilitiesObj);
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
}

