namespace PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
public interface IDoctorAvailabilityRepository
{
    Task<IResponse> AddEditDoctorAvaibality(AddEditDoctorAvailabilityWithUserId model);
    Task<IResponse> GetByIdDoctorAvaibality(Guid Id);
    Task<IResponse> GetAllByProc(GetDoctorAvailabiltiesList model);
    Task<IResponse> ActiveInActive(ActiveInActiveDoctorAvailability model);
}

