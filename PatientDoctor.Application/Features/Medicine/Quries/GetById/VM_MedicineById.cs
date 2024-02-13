using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetById
{
    public class VM_MedicineById
    {
        public string MedicineName { get; set; }
        public Guid MedicineTypeId { get; set; }
        public string DoctorId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
