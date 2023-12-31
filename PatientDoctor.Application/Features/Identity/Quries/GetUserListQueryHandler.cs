
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
    public class GetUserListQueryHandler : IRequestHandler<GetUserList, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public GetUserListQueryHandler(IIdentityRepository identityRepository)
        {
            this._identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public Task<IResponse> Handle(GetUserList request, CancellationToken cancellationToken)
        {
            var UserList = _identityRepository.GetAllByProc(request);
            return UserList;
        }
    }
}
