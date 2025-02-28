using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Administrator.Commands.Register;
public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, IResponse>
{
    private readonly IAdministratorRepository _administratorRepository;

    public UserRegisterCommandHandler(IAdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository ?? throw new ArgumentNullException(nameof(administratorRepository));
    }
    public async Task<IResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        return await _administratorRepository.UserRegister(request);
    }
}

