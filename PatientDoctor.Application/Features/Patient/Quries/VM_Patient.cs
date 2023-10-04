using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class VM_Patient : ListingLogFields
    {
        public Guid PatientId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DoctorName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string DoctorPhoneNumber { get; set; }
        public string City { get; set; }
        public string BloodType { get; set; }
        public string Cnic { get; set; }
        public int Status { get; set; }
        public string MaritalStatus { get; set; }
    }
}
