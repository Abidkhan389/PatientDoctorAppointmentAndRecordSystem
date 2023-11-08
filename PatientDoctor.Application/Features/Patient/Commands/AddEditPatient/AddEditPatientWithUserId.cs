using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.AddEditPatient
{
    public class AddEditPatientWithUserId:TableParam, IRequest<IResponse>
    {
        public AddEditPatientCommand AddEditPatientObj { get; }
        public Guid UserId { get; }

        public AddEditPatientWithUserId(AddEditPatientCommand model, Guid Userid)
        {
            AddEditPatientObj = model;
            this.UserId = Userid;
        }
    }
}
