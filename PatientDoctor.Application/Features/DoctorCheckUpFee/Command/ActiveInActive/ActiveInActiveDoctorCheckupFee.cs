using System;
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.ActiveInActive
{
	public class ActiveInActiveDoctorCheckupFee : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}

