using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
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
        public Userdetail(AddEditUserWithCreatedOrUpdatedById model)
        {
            // Initialize properties here if needed
            this.CreatedOn = DateTime.UtcNow;
            this.Cnic = model.addEditUsermodel.Cnic;
            this.City =model.addEditUsermodel.City;
            this.CreatedBy = model.UserId;
        }
        public void Initialize(ApplicationUser model)
        {
            //this.UserId = Guid.TryParse(model.Id, out var userIdGuid) ? userIdGuid : Guid.Empty;
            this.UserId = model.Id;
            this.UserDetailId = Guid.NewGuid();
        }
    }
}
