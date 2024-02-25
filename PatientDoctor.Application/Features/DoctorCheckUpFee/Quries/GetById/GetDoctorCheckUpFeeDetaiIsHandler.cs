using System;
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetById
{
	public class GetDoctorCheckUpFeeDetaiIsHandler : IRequestHandler<GetDocterCheckupFeeById, IResponse>
    {
        private readonly IDoctorCheckUpFeeRepository _doctorCheckUpFeeRepository;
        public GetDoctorCheckUpFeeDetaiIsHandler(IDoctorCheckUpFeeRepository doctorCheckUpFeeRepository)
		{
            _doctorCheckUpFeeRepository = doctorCheckUpFeeRepository ?? throw new ArgumentNullException(nameof(doctorCheckUpFeeRepository));
        }
        public async Task<IResponse> Handle(GetDocterCheckupFeeById request, CancellationToken cancellationToken)
        {
            return await _doctorCheckUpFeeRepository.GetDoctorCheckUpFeeById(request.Id);
        }
    }
}

