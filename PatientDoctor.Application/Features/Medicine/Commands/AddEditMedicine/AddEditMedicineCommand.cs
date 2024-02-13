using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine
{
    public class AddEditMedicineCommand : IRequest<IResponse>
    {
        public Guid? Id { get; set; }
        public Guid MedicineTypeId { get; set; }
        public string DoctorId { get; set; }
        public string MedicineName { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExperiyDate { get; set; }
    }
}
