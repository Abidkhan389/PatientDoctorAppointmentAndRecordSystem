
namespace PatientDoctor.Application.Contracts.Persistance.Patient
{
    public interface IPatientRepository
    {
        Task<IResponse> GetPatientById(GetPatientById Id);
        Task<IResponse> AddEditPatient(AddEditPatientWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActivePatients model);
        Task<IResponse> GetAllByProc(GetPatientListWithUser model);
        Task<IResponse> GetAllPatientAppoitmentWithDoctorProc(GetPatientAppoitmentListWithDocter model);
        Task<IResponse> AddEditPatientDescription(AddPatientDescriptionCommand model);
        Task<IResponse> GetPatientDescriptionById(GetPatientDescription model);
        Task<IResponse> GetPatientsRecordWithDoctorProc(GetPatientRecordListWithDoctor model);
        Task<IResponse> GetPatientDetailsForPdf(GetPatientDetailsForPdfRequest model);
        Task<IResponse> GetDoctorAppointmentsSlotsOfDay(GetDoctorTimeSlotsByDayIdAndDoctorId model);
        Task<IResponse> patientDiscount(PatientDiscount model);
        Task<IResponse> UpdatePatientAppointmentStatus(UpdatePatientAppointmentStatusCommand model, CancellationToken cancellationToken);
    }
}
