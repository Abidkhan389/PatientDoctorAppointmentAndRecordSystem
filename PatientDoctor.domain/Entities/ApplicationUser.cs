using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string PasswordSalt { get; set; }
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
    }
}
