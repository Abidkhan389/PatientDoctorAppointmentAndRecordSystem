using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.ActiveInActive
{
    public class ActiveInActiveHandlerQuery : IRequestHandler<ActiveInActive, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public ActiveInActiveHandlerQuery(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(ActiveInActive request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.ActiveInActiveUser(request);
            return user;
        }
    }
}
