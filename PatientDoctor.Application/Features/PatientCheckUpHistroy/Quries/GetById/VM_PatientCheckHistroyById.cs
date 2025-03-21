
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
public class VM_PatientCheckHistroyById
{
    public Guid? PrescriptionId { get; set; }
    public Guid PatientId { get; set; }
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

    public virtual ICollection<VM_PrescriptionMedicine> Medicine { get; set; } = new List<VM_PrescriptionMedicine>();
}
public class VM_PrescriptionMedicine
{
    public Guid? Id { get; set; }
    public Guid MedicineId { get; set; }
    public Guid PotencyId { get; set; }
    // Timing Information
    public bool? Morning { get; set; }
    public bool? Afternoon { get; set; }
    public bool? Evening { get; set; }
    public bool? Night { get; set; }
    public bool? RepeatEveryHours { get; set; }
    public bool? RepeatEveryTwoHours { get; set; }

    // New Property for Duration
    public int? DurationInDays { get; set; }

}

