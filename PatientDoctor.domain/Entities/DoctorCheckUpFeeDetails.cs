using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees;
using PatientDoctor.domain.Entities.Public;

namespace PatientDoctor.domain.Entities
{
	[Table("DoctorCheckUpFeeDetails",Schema = "Admin")]
	public class DoctorCheckUpFeeDetails : LogFields
	{
		public DoctorCheckUpFeeDetails()
		{

		}
		public DoctorCheckUpFeeDetails(AddEditDoctorCheckUpFeeCommands model,Guid UserId)
		{
			Id = Guid.NewGuid();
			DoctorFee = model.DoctorFee;
			DoctorId = model.DoctorId;
			CreatedBy = UserId;
			CreatedOn = DateTime.UtcNow;
			Status = 1;
		}
		[Key]
		public Guid Id { get; set; }
		public int DoctorFee { get; set; }
		public string DoctorId { get; set; }
        public int Status { get; set; }
    }
}

