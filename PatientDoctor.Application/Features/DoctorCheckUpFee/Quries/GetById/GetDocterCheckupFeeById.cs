using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetById
{
	public class GetDocterCheckupFeeById : IRequest<IResponse>
	{
        public Guid Id { get; set; }

    }
}

