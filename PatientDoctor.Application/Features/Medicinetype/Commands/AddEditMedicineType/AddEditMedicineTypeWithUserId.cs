using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;

namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    public class AddEditMedicineTypeWithUserId : TableParam, IRequest<IResponse>
    {
        public AddEditMedicineTypeCommand addEditMedicineTypeObj { get; }
        public Guid UserId { get; }
        public AddEditMedicineTypeWithUserId(AddEditMedicineTypeCommand model, Guid Userid)
        {
            addEditMedicineTypeObj = model;
            this.UserId = Userid;
        }
    }
}
