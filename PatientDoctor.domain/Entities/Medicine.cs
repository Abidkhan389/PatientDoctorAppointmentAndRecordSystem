using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.domain.Entities.Public;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.domain.Entities
{
    [Table("Medicine", Schema = "Admin")]
    public class Medicine : LogFields
    {
        public Medicine()
        {
            
        }
        public Medicine(AddEditMedicineCommand model,Guid UserId)
        {
            this.Id= Guid.NewGuid();
            this.MedicineName= model.MedicineName;
            this.MedicineTypeId = model.MedicineTypeId;
            this.DoctorId=model.DoctorId;
            this.StartingDate=model.StartingDate;
            this.ExperiyDate=model.ExperiyDate;
            this.Status = 1;
            this.CreatedBy = UserId;
            this.CreatedOn = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { get; set; }
        public string MedicineName { get; set; }
        public Guid MedicineTypeId { get; set; }
        public string DoctorId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExperiyDate { get; set; }
        public int Status { get; set; }


    }
}
