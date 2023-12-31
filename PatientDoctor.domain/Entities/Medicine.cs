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
    [Table("Medicine", Schema = "Admin")]
    public class Medicine : LogFields
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExperiyDate { get; set; }
        public int Status { get; set; }


    }
}
