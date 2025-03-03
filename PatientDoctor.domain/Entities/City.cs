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
    [Table("City", Schema = "Admin")]
    public class City : LogFields
    {
        public Guid Id { get; set; }

        public string CityName { get; set; }

        public Guid ProvinceID { get; set; } // Foreign Key

        public int Status { get; set; }

        // Navigation Property
        public virtual Province Province { get; set; }

    }
}
