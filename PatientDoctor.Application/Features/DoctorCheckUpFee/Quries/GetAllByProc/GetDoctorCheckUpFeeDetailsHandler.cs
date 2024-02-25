using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc
{
	public class GetDoctorCheckUpFeeDetailsHandler : IRequestHandler<GetDoctorCheckUpFeeDetailsList, IResponse>
    {
        private readonly IDoctorCheckUpFeeRepository _doctorCheckUpFeeRepository;
        public GetDoctorCheckUpFeeDetailsHandler(IDoctorCheckUpFeeRepository doctorCheckUpFeeRepository)
		{
            _doctorCheckUpFeeRepository = doctorCheckUpFeeRepository ?? throw new ArgumentNullException(nameof(doctorCheckUpFeeRepository));
        }
        public async Task<IResponse> Handle(GetDoctorCheckUpFeeDetailsList request, CancellationToken cancellationToken)
        {
            return await _doctorCheckUpFeeRepository.GetAllByProc(request);
        }
    }
}

