
namespace PatientDoctor.Infrastructure.Utalities;
    public class AuthenticatedUser
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       // public ApplicationUser User { get; set; } 
       public string Id { get; set; }
       public string Email { get; set; }
    }

