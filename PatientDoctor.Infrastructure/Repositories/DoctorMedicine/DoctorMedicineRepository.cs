using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
using PatientDoctor.Application.Features.DoctorMedicine.Command;
using PatientDoctor.Application.Features.DoctorMedicine.Query;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Security.Claims;

namespace PatientDoctor.Infrastructure.Repositories.DoctorMedicine;
public class DoctorMedicineRepository(DocterPatiendDbContext _context, IResponse _response, ICountResponse _countResp, UserManager<ApplicationUser> _userManager) : IDoctorMedicineRepository
{
    public async Task<IResponse> AddEditDoctorMedicine(AddEditDoctorMedicineCommand model)
    {
        try
        {
            var userInfo = await _userManager.FindByIdAsync(model.UserId);
            var doctormedicineManagementToUpdate = await _context.DoctorMedicines
                    .Where(s => s.MedicineId == model.MedicineId)
                    .ToListAsync();
            if (!doctormedicineManagementToUpdate.Any())
            {
                List<DoctorMedicines> doctormedicineList = new List<DoctorMedicines>();
                foreach (var doctor in model.DoctorIds)
                {
                    var DoctorMedicineObj = new DoctorMedicines(model, doctor.DoctorId, userInfo.Id);
                    doctormedicineList.Add(DoctorMedicineObj);
                }
                await _context.DoctorMedicines.AddRangeAsync(doctormedicineList);
                await _context.SaveChangesAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataSaved;
            }
            else
            {
                // Prepare lists for removal and addition
                var doctorMedicineToRemove = new List<DoctorMedicines>();
                var doctorMedicineToAdd = new List<DoctorMedicines>();

                // Create a lookup for IDs from the model's facultySubjectIds
                var studentIdsLookup = model.DoctorIds.ToDictionary(s => s.DoctorId);

                // Add items to remove list if they no longer exist in the new selection
                foreach (var item in doctormedicineManagementToUpdate)
                {
                    if (!studentIdsLookup.ContainsKey(item.DoctorId))
                    {
                        doctorMedicineToRemove.Add(item);
                    }
                }

                // Remove items that are no longer in the model
                if (doctorMedicineToRemove.Any())
                {
                    _context.DoctorMedicines.RemoveRange(doctorMedicineToRemove);
                }

                // Find new items to add by excluding existing IDs
                var existingClassRoomStudentManagementIds = doctormedicineManagementToUpdate.Select(rs => rs.DoctorId).ToHashSet();
                var newClassRoomStudentManagementDtos = model.DoctorIds
                    .Where(id => !existingClassRoomStudentManagementIds.Contains(id.DoctorId))
                    .ToList();

                // Add new items using the copy constructor
                foreach (var item in newClassRoomStudentManagementDtos)
                {
                    var newEntity = new DoctorMedicines
                    {
                        DoctorId = item.DoctorId, // Assuming ClassRoomId needs to be set
                        MedicineId=model.MedicineId
                        
                    };
                    doctorMedicineToAdd.Add(newEntity);
                }

                // Add all new entities to the context
                if (doctorMedicineToAdd.Any())
                {
                    await _context.DoctorMedicines.AddRangeAsync(doctorMedicineToAdd);
                }
                await _context.SaveChangesAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
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

    public async Task<IResponse> GetDoctorMedicineById(Guid MedicineId)
    {
        try
        {
            // Fetch data based on ClassRoomId
            var doctorMedicinList = await _context.DoctorMedicines
                .Where(x => x.MedicineId == MedicineId)
                .ToListAsync();

            if (doctorMedicinList == null || !doctorMedicinList.Any())
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Doctor Medicine Mapping Not Found");
            }
            else
            {

                // Grouping by ClassRoomId and mapping to ClassRoomFacultySubjectDto
                var DoctorMedicineDto = doctorMedicinList
                    .GroupBy(x => x.MedicineId)
                    .Select(group => new VM_DoctorMedicine
                    {
                        MedicineId = group.Key,
                        DoctorIds = group.Select(item => new DoctorIds
                        {
                            
                            DoctorId = item.DoctorId
                        }).ToList()
                    })
                    .FirstOrDefault(); // Select the first or default item to return a single object

                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
                _response.Data = DoctorMedicineDto;
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
}

