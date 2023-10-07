using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<AddEditUserWithCreatedOrUpdatedById, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public RegisterUserCommandHandler(IIdentityRepository identityRepository)
        {
            this._identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public Task<IResponse> Handle(AddEditUserWithCreatedOrUpdatedById request, CancellationToken cancellationToken)
        {
            var register= _identityRepository.AddEditUser(request);
            return register;
        }
    }
}
