using MediatR;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.RegisterUser
{
    public  class AddEditUserWithCreatedOrUpdatedById : TableParam, IRequest<IResponse>
    {
        public AddEditUserCommands addEditUsermodel { get; }
        public Guid UserId { get; }

        public AddEditUserWithCreatedOrUpdatedById(AddEditUserCommands model, Guid UserId)
        {
            this.addEditUsermodel = model;
            this.UserId = UserId;
        }
    }
}
