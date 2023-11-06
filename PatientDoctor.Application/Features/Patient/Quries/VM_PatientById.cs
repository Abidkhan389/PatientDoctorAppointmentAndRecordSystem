using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class VM_PatientById
    {
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppoitmentTime { get; set; }
        public DateTime DateofBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string BloodType { get; set; }
        public string Cnic { get; set; }
        public string MaritalStatus { get; set; }
    }
}
