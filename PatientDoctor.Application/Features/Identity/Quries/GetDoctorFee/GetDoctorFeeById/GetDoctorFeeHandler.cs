using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Identity.Quries.GetDoctorFee.GetDoctorFeeById;
public class GetDoctorFeeHandler : IRequestHandler<GetDoctorFee, IResponse>
{
    private readonly IIdentityRepository _identityRepository;

    public GetDoctorFeeHandler(IIdentityRepository identityRepository)
    {
        this._identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
    }
    public async Task<IResponse> Handle(GetDoctorFee request, CancellationToken cancellationToken)
    {
        return await _identityRepository.GetDoctorFee(request);
    }
}

