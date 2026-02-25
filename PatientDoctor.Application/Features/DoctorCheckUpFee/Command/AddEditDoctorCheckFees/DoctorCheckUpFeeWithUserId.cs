
namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees
{
	public class DoctorCheckUpFeeWithUserId : IRequest<IResponse>
    {
        public AddEditDoctorCheckUpFeeCommands addEditDoctorCheckUpFee { get; }
        public Guid UserId { get; }
        public DoctorCheckUpFeeWithUserId(AddEditDoctorCheckUpFeeCommands model, Guid Userid)
        {
            addEditDoctorCheckUpFee = model;
            this.UserId = Userid;
        }
	}
}

