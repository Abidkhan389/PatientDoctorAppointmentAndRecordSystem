using Microsoft.AspNetCore.Identity;
using PatientDoctor.domain.Entities.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string? PasswordSalt { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int Status { get; set; }
        public string RoleName { get; set; }
        public string? ProfilePicture { get; set; }
        //public string Photo { get; set; }
        public ApplicationUser()
        {
            // Initialize properties here if needed
            this.IsSuperAdmin = false;
            this.PasswordHash = string.Empty;
        }
        public ICollection<DoctorMedicines> DoctorMedicines { get; set; } = new List<DoctorMedicines>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        // Navigation
        public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public virtual ICollection<DoctorAssistant> Assistants { get; set; }   // If Doctor
        public virtual ICollection<DoctorAssistant> AssignedDoctor { get; set; } // // If Assistant
        public virtual Userdetail UserDetails { get; set; }
    }
}
