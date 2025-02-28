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
    [Table("MedicinePotency", Schema = "Admin")]
    public class MedicinePotency : LogFields
    {
        public Guid Id { get; set; }

        public string Potency { get; set; }

        public Guid MedicineTypeId { get; set; } // Foreign Key

        public int Status { get; set; }

        // Navigation Property
        public virtual MedicineType MedicineType { get; set; }

        public MedicinePotency() { }

        public MedicinePotency(string potency, Guid medicineTypeId, Guid userId)
        {
            Id = Guid.NewGuid();
            Potency = potency;
            MedicineTypeId = medicineTypeId;
            Status = 1;
            CreatedBy = userId;
            CreatedOn = DateTime.UtcNow;
        }
    }
}
