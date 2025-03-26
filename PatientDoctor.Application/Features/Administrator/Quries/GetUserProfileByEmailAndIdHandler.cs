using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Administrator.Quries;
public class GetUserProfileByEmailAndIdHandler(IAdministratorRepository _administratorRepository) : IRequestHandler<GetUserProfileByEmailAndId, IResponse>
{
    private readonly IAdministratorRepository _repo =
        _administratorRepository ?? throw new ArgumentNullException(nameof(_administratorRepository));

    public async Task<IResponse> Handle(GetUserProfileByEmailAndId request, CancellationToken cancellationToken)
    {
        return await _administratorRepository.GetUserProfileByEmailAndId(request);
    }
}

