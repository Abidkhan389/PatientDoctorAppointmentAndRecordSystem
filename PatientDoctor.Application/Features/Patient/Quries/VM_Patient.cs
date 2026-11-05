
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class VM_Patient : ListingLogFields
    {
        public Guid PatientId { get; set; }
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string DoctorPhoneNumber { get; set; }
        public string City { get; set; }
        public string BloodType { get; set; }
        public string Cnic { get; set; }
        public int Status { get; set; }
        public int CheckUpStatus { get; set; }
        public string MaritalStatus { get; set; }
        public string? TimeSlot { get; set; }
    }

