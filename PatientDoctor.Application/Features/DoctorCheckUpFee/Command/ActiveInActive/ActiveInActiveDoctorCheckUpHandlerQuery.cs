using System;
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.ActiveInActive
{
	public class ActiveInActiveDoctorCheckUpHandlerQuery : IRequestHandler<ActiveInActiveDoctorCheckupFee, IResponse>
    {
        private readonly IDoctorCheckUpFeeRepository _doctorCheckUpFeeRepository;
        public ActiveInActiveDoctorCheckUpHandlerQuery(IDoctorCheckUpFeeRepository doctorCheckUpFeeRepository) 
		{
            _doctorCheckUpFeeRepository= doctorCheckUpFeeRepository ?? throw new ArgumentNullException(nameof(doctorCheckUpFeeRepository));
        }
        public async  Task<IResponse> Handle(ActiveInActiveDoctorCheckupFee request, CancellationToken cancellationToken)
        {
            return await _doctorCheckUpFeeRepository.ActiveInActive(request);
        }
    }
}

