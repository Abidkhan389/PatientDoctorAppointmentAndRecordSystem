using MediatR;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Doctor_Availability.Commands
{
    public class AddEditDoctorAvailabilityWithUserId : TableParam, IRequest<IResponse>
    {
        public AddEditDoctorAvailabilityCommands AddEditDoctorAvailabilityObj { get; }
        public Guid UserId { get; }

        public AddEditDoctorAvailabilityWithUserId(AddEditDoctorAvailabilityCommands model, Guid Userid)
        {
            AddEditDoctorAvailabilityObj = model;
            this.UserId = Userid;
        }
    }
}
