using PatientDoctor.domain.Entities.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.domain.Entities
{
    //Patient Table for Admin to add Patient in Admin Schema
    [Table("Patient", Schema = "Admin")]
    public class Patient
    {
        [Key]
        public Guid PatientId { get; set; }
        public string FirstName { get; set; }
        public int Status { get; set; }
        public string Cnic { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoctoerId { get; set; }
        public int Age { get; set; }
        public string? Description { get; set; }
    }
}
