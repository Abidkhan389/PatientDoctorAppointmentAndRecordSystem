using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries
{
    public class VM_MedicineType : ListingLogFields
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public string? TabletMg {  get; set; }
        public int Status { get; set; }
    }
}
