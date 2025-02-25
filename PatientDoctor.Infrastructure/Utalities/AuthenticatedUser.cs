using PatientDoctor.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Utalities
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ApplicationUser User { get; set; } 
    }
}
