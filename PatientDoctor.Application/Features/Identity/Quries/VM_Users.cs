using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class VM_Users:ListingLogFields
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string Cnic { get; set; }
        public string City { get; set; }
        public int Status { get; set; }
        public List<string> Roles { get; set; }
       
    }
}
