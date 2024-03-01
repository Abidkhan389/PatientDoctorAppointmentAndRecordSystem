using System;
using MediatR;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;

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

