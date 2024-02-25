using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees
{
	public class AddEditDoctorCheckUpFeeCommands : IRequest<IResponse>
	{
		public AddEditDoctorCheckUpFeeCommands()
		{
		}
        public Guid? Id { get; set; }
        public int DoctorFee { get; set; }
        public string DoctorId { get; set; }
    }
}

