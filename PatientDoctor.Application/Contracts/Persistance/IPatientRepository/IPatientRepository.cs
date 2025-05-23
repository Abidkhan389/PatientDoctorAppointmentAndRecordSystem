﻿using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using PatientDoctor.Application.Features.Patient.Commands.PatientDiscount;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor;
using PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
using PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf;
using PatientDoctor.Application.Helpers;

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
    }
}
