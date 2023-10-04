using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserByIdHandlerQuery : IRequestHandler<GetUserById, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public GetUserByIdHandlerQuery(IIdentityRepository identityRepository)
        {
            this._identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.GetUserById(request);
            return user;
        }
    }
}
