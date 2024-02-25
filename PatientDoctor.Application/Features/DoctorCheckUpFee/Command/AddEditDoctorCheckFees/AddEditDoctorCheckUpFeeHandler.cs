using System;
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees
{
	public class AddEditDoctorCheckUpFeeHandler :IRequestHandler<DoctorCheckUpFeeWithUserId, IResponse>
	{
        private readonly IDoctorCheckUpFeeRepository _doctorCheckUpFeeRepository;
        public AddEditDoctorCheckUpFeeHandler(IDoctorCheckUpFeeRepository doctorCheckUpFeeRepository)
        {
            _doctorCheckUpFeeRepository = doctorCheckUpFeeRepository ?? throw new ArgumentNullException(nameof(doctorCheckUpFeeRepository));
        }

        public async Task<IResponse> Handle(DoctorCheckUpFeeWithUserId request, CancellationToken cancellationToken)
        {
            return await _doctorCheckUpFeeRepository.AddEditDoctorCheckUpFee(request);
        }
    }
}

