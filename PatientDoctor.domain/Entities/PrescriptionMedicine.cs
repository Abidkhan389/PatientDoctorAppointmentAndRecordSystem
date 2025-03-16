using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.domain.Entities;
    [Table("PrescriptionMedicine", Schema = "Admin")]
    public class PrescriptionMedicine
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("MedicineId")]
        public Guid MedicineId { get; set; }
        [ForeignKey("Prescription")]
        public Guid PrescriptionId { get; set; }

        // Timing Information
        public bool? Morning { get; set; }
        public bool? Afternoon { get; set; }
        public bool? Evening { get; set; }
        public bool? Night { get; set; }
        public bool? RepeatEveryHours { get; set; }
        public bool? RepeatEveryTwoHours { get; set; }

        // New Property for Duration
        public int? DurationInDays { get; set; }

        // Navigation Property
        public virtual Prescription Prescription { get; set; }
        public virtual Medicine Medicine { get; set; }
    }


