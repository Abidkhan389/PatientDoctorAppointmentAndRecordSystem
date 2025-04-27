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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int? Fee { get; set; }
        public bool? IsNotified { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Userdetail()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
                
        }
        public Userdetail(AddEditUserWithCreatedOrUpdatedById model, ApplicationUser user)
        {
            // Initialize properties here if needed
            this.UserId = user.Id;
            this.UserDetailId = Guid.NewGuid();
            this.CreatedOn = DateTime.UtcNow;
            this.Cnic = model.addEditUsermodel.Cnic;
            this.City =model.addEditUsermodel.City;
            this.CreatedBy = model.UserId;
            this.FirstName = model.addEditUsermodel.FirstName;
            this.LastName = model.addEditUsermodel.LastName;
            this.Fee = model.addEditUsermodel.Fee;
        }
        //public void Initialize(ApplicationUser model)
        //{
        //    //this.UserId = Guid.TryParse(model.Id, out var userIdGuid) ? userIdGuid : Guid.Empty;
        //    this.UserId = model.Id;
        //    this.UserDetailId = Guid.NewGuid();
        //}
    }
}
