using PatientDoctor.domain.Entities.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    public class Userdetail: LogFields
    {
        [Key]
        public Guid UserDetailId { get; set; }
        public string UserId { get; set; }
        public string Cnic { get; set; }
        public string City { get; set; }
        public Userdetail(string Cnic,string City)
        {
            // Initialize properties here if needed
            this.CreatedOn = DateTime.UtcNow;
            this.Cnic = Cnic;
            this.City = City;

        }
        public void Initialize(ApplicationUser model)
        {
            //this.UserId = Guid.TryParse(model.Id, out var userIdGuid) ? userIdGuid : Guid.Empty;
            this.UserId = model.Id;
            this.UserDetailId = Guid.NewGuid();
        }
    }
}
