using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.AddEditPatient
{
    public class AddEditPatiendCommandHandler : IRequestHandler<AddEditPatientCommand, IResponse>
    {
        private readonly IPatientRepository _patientRepository;

        public AddEditPatiendCommandHandler(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }
        public async Task<IResponse> Handle(AddEditPatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.AddEditPatient(request);
            return patient;
        }
    }
}
