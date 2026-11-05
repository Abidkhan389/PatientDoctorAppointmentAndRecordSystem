
namespace PatientDoctor.domain.Entities;
    //Patient Table for Admin to add Patient in Admin Schema
    [Table("Patient", Schema = "Admin")]
    public class Patient
    {
        [Key]
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public int Status { get; set; }
        public string Cnic { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoctoerId { get; set; }
        public int Age { get; set; }
        public string? Description { get; set; }
        public string? TrackingNumber { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }

