using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
using PatientDoctor.domain.Entities.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    [Table("MedicineType", Schema = "Admin")]
    public class MedicineType : LogFields
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public int Status { get; set; }
        public MedicineType() { }
        public MedicineType(AddEditMedicineTypeCommand model, Guid UserId)
        {
            this.Id= Guid.NewGuid();
            this.Status = 1;
            this.TypeName = model.TypeName;
            this.CreatedBy = UserId;
            this.CreatedOn = DateTime.UtcNow;
            this.CreatedBy = UserId;
            this.CreatedOn=DateTime.UtcNow;
        }

    }
}
