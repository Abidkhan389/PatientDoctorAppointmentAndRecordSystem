using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Administrator.Commands.ResetPassword
{
    public  class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResponse>
    {
        private readonly IAdministratorRepository _administratorRepository;

        public ResetPasswordCommandHandler(IAdministratorRepository administratorRepository)
        {
            this._administratorRepository = administratorRepository ?? throw new ArgumentNullException(nameof(administratorRepository));
        }
        public async Task<IResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _administratorRepository.ResetPassword(request);
        }
    }
}
