using PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Helpers;


namespace PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
public interface IDoctorAvailabilityRepository
{
    Task<IResponse> AddEditDoctorAvaibality(AddEditDoctorAvailabilityWithUserId model);
    Task<IResponse> GetByIdDoctorAvaibality(Guid Id);
    Task<IResponse> GetAllByProc(GetDoctorAvailabiltiesList model);
    Task<IResponse> ActiveInActive(ActiveInActiveDoctorAvailability model);
}

