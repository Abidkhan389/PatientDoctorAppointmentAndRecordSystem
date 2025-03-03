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
    [Table("Province", Schema = "Admin")]
    public class Province : LogFields
    {
        public Guid ID { get; set; }
        public string ProvinceName { get; set; }
        public int Status { get; set; }

    }
}
