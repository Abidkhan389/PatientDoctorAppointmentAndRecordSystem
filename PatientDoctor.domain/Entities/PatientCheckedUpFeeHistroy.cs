using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription.PatientCheckedUpFeeHistroy;
using PatientDoctor.domain.Entities.Public;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.domain.Entities
{
    [Table("PatientCheckedUpFeeHistroy", Schema = "Admin")]
    public class PatientCheckedUpFeeHistroy : LogFields
    {
        public PatientCheckedUpFeeHistroy()
        {
            
        }
        public PatientCheckedUpFeeHistroy(PatientCheckedUpFeeHistroyDto model)
        {
            Id = Guid.NewGuid();
            DoctorId = model.DoctorId;
            DoctorName = model.DoctorName;
            DoctorEmail = model.DoctorEmail;
            DoctorNumber = model.DoctorNumber;
            PatientId = model.PatientId;
            PatientName = model.PatientName;
            PatientNumber = model.PatientNumber;
            PatientCnic = model.PatientCnic;
            CheckUpFee = model.CheckUpFee;
            CreatedBy = !string.IsNullOrEmpty(model.DoctorId) ? new Guid(model.DoctorId) : (Guid?)null;
            CreatedOn = DateTime.Now;
        }
        [Key]
        public Guid Id { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string? DoctorNumber { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set;}
        public string? PatientNumber { get; set; }
        public string PatientCnic {  get; set; }
        public int CheckUpFee { get; set; }
    }
}
