using FluentValidation;

namespace PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor
{
    public class GetPatientRecordListWithDoctorValidator : AbstractValidator<GetPatientRecordListWithDoctor>
    {
        public GetPatientRecordListWithDoctorValidator()
        {
            RuleFor(request => request.getPatientRecordList.DoctorName)
                .Must(doctorname=> doctorname == null || !string.IsNullOrWhiteSpace(doctorname))
                .NotEmpty().WithMessage("White Space is not Allowed");

            RuleFor(request => request.getPatientRecordList.PatientName)
           .Must(patientName => patientName == null || !string.IsNullOrWhiteSpace(patientName))
           .WithMessage("PatientName cannot be empty or whitespace.");

            RuleFor(request => request.getPatientRecordList.PatientCnic)
                .Must(patientCnic => patientCnic == null || !string.IsNullOrWhiteSpace(patientCnic))
                .WithMessage("PatientCnic cannot be empty or whitespace.");

            RuleFor(request => request.getPatientRecordList.PatientCheckUpDateFrom)
           .Must(patientCheckUpDateFrom => patientCheckUpDateFrom == null || patientCheckUpDateFrom <= DateTime.Now)
           .WithMessage("PatientCheckUpDateFrom must be null or less than or equal to the current date.");

            RuleFor(request => request.getPatientRecordList.PatientCheckUpDateTo)
                .Must(patientCheckUpDateTo => patientCheckUpDateTo == null || patientCheckUpDateTo <= DateTime.Now)
                .WithMessage("PatientCheckUpDateTo must be null or less than or equal to the current date.")
                .When(request => request.getPatientRecordList.PatientCheckUpDateTo != null);

            RuleFor(request => request.getPatientRecordList.PatientCheckUpDateTo)
                .Must((request, patientCheckUpDateTo) => patientCheckUpDateTo == null || request.getPatientRecordList.PatientCheckUpDateFrom == null || patientCheckUpDateTo >= request.getPatientRecordList.PatientCheckUpDateFrom)
                .WithMessage("PatientCheckUpDateTo must be greater than or equal to PatientCheckUpDateFrom.")
                .When(request => request.getPatientRecordList.PatientCheckUpDateTo != null && request.getPatientRecordList.PatientCheckUpDateFrom != null);
        }
    
    }
}
