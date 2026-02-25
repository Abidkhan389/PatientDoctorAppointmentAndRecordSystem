
namespace PatientDoctor.domain.Entities;
    [Table("PatientDetails", Schema = "Admin")]
    public class PatientDetails: LogFields
    {
        [Key]
        public Guid PatiendDetailsId { get; set; }
        public Guid PatientId { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string? BloodType { get; set; }
        public int Status {  get; set; }
        public string? MaritalStatus { get; set; }
        public int CheckUpStatus { get; set; }

    }

