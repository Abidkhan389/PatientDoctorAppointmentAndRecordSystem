using PatientDoctor.Application.Features.DoctorMedicine.Command;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.domain.Entities.Public;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace PatientDoctor.domain.Entities;
public class DoctorMedicines : LogFields
{
    public Guid Id { get; set; }
    [ForeignKey("Doctor")]
    public string DoctorId { get; set; }
    public Guid MedicineId { get; set; }

    // Navigation properties (optional for easier data retrieval)
    public ApplicationUser Doctor { get; set; }
    public Medicine Medicine { get; set; }

    public DoctorMedicines()
    {        
    }
    public DoctorMedicines(AddEditDoctorMedicineCommand model, string doctor,string createdBy)
    {
        this.DoctorId = doctor;
        this.MedicineId =model.MedicineId;
        this.CreatedBy = Guid.Parse(createdBy);
        this.CreatedOn = DateTime.Now;
    }
}

