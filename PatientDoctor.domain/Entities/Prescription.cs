using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using PatientDoctor.domain.Entities.Public;
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
    public class Prescription : LogFields
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        [ForeignKey("Patient")]
        public Guid PatientId { get; set; }
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }

        // Eye Examination Details
        public string? LeftVision { get; set; }
        public string? RightVision { get; set; }
        public string? LeftMG { get; set; }
        public string? RightMG { get; set; }
        public string? LeftEOM { get; set; }
        public string? RightEOM { get; set; }
        public string? LeftOrtho { get; set; }
        public string? RightOrtho { get; set; }
        public string? LeftTension { get; set; }
        public string? RightTension { get; set; }
        public string? LeftAntSegment { get; set; }
        public string? RightAntSegment { get; set; }
        public string? LeftDisc { get; set; }
        public string? RightDisc { get; set; }
        public string? LeftMacula { get; set; }
        public string? RightMacula { get; set; }
        public string? LeftPeriphery { get; set; }
        public string? RightPeriphery { get; set; }
        public int Status { get; set; }

        // Other Details
        public string? Complaint { get; set; }
        public string? Diagnosis { get; set; }
        public string? Plan { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual ICollection<PrescriptionMedicine> Medicines { get; set; } = new List<PrescriptionMedicine>();
        public Prescription()
        {
            
        }
        public Prescription(AddPatientDescriptionCommand model)
        {
            this.PatientId = model.PatientId;
            this.DoctorId = model.DoctorId;
            this.LeftVision = model.LeftVision;
            this.RightVision = model.RightVision;
            this.LeftMG = model.LeftMG;
            this.RightMG = model.RightMG;
            this.LeftEOM = model.LeftEOM;
            this.RightEOM = model.RightEom;
            this.LeftOrtho = model.LeftOrtho;
            this.RightOrtho = model.RightOrtho;
            this.LeftTension = model.LeftTension;
            this.RightTension = model.RightTension;
            this.LeftAntSegment = model.LeftAntSegment;
            this.RightAntSegment = model.RightAntSegment;
            this.LeftDisc = model.LeftDisc;
            this.RightDisc = model.RightDisc;
            this.LeftMacula = model.LeftMacula;
            this.RightMacula = model.RightMacula;
            this.LeftPeriphery = model.LeftPeriphery;
            this.RightPeriphery = model.RightPeriphery;
            this.Complaint = model.ComplaintOf;
            this.Diagnosis = model.Diagnosis;
            this.Plan = model.Plan;
            this.CreatedBy = model.UserId;
            this.CreatedOn = DateTime.UtcNow;
            this.Status = 1;
            this.CreatedAt = DateTime.UtcNow;
            // ✅ Add medicines to Prescription
            if (model.medicine != null && model.medicine.Any())
            {
                this.Medicines = model.medicine.Select(m => new PrescriptionMedicine
                {
                    MedicineId = m.MedicineId,
                    PotencyId=m.PotencyId,
                    DurationInDays = m.DurationInDays,
                    Morning = m.Morning ,
                    Afternoon = m.Afternoon,
                    Evening = m.Evening,
                    Night = m.Night ,
                    RepeatEveryHours = m.RepeatEveryHours ,
                    RepeatEveryTwoHours = m.RepeatEveryTwoHours
                }).ToList();
            }
        }
    }

}
