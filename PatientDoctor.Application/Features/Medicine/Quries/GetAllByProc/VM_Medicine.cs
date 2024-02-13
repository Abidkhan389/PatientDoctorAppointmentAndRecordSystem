using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc
{
    public class VM_Medicine : ListingLogFields
    {
        public Guid Id { get; set; }
        public string MedicineName { get; set; }
        public string MedicineTypeName{ get; set; }
        public string DoctorName { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; }

    }
}
