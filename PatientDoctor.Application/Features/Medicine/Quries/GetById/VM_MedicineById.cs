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
        public Guid MedicineTypePotencyId { get; set; }
        public string? MedicineTypePotencyName { get; set; }
        public string? MedicineTypeName { get; set; }
        public string DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string Potency { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    public class VM_MedicineTypePotency
    {
        public Guid MedicineTypePotencyId { get; set; }
        public Guid MedicineTypeId { get; set; }
        public string Potency { get; set; }

    }
}
