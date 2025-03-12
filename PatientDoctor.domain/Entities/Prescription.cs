using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    [Table("Prescription", Schema = "Admin")]
    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        [ForeignKey("Patient")]
        public Guid PatientId { get; set; }
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }

        // Eye Examination Details
        public string LeftVision { get; set; }
        public string RightVision { get; set; }
        public string LeftMG { get; set; }
        public string RightMG { get; set; }
        public string LeftEOM { get; set; }
        public string RightEOM { get; set; }
        public string LeftOrtho { get; set; }
        public string RightOrtho { get; set; }
        public string LeftTension { get; set; }
        public string RightTension { get; set; }
        public string LeftAntSegment { get; set; }
        public string RightAntSegment { get; set; }
        public string LeftDisc { get; set; }
        public string RightDisc { get; set; }
        public string LeftMacula { get; set; }
        public string RightMacula { get; set; }
        public string LeftPeriphery { get; set; }
        public string RightPeriphery { get; set; }

        // Other Details
        public string Complaint { get; set; }
        public string Diagnosis { get; set; }
        public string Plan { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual ICollection<PrescriptionMedicine> Medicines { get; set; } = new List<PrescriptionMedicine>();
    }
}
