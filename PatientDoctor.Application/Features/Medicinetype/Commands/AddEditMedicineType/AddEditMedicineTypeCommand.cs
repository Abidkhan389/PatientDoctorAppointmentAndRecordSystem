using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    public class AddEditMedicineTypeCommand : IRequest<IResponse>
    {
        public Guid? MedicineTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
