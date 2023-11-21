using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription
{
    public class AddPatientDescriptionCommand : IRequest<IResponse>
    {
        public Guid PatientId { get; set; }
        public string Description { get; set; }
        public int Eye1 { get; set; }
        public int Eye2 { get; set; }
        public int DistanceEye1 { get; set; }
        public int DistanceEye2 { get; set; }
        public string Eye1SidePoint { get; set; }
        public string Eye2SidePoint { get; set; }
        public List<MedicineOptions> medicine { get; set; }
    }
    public class MedicineOptions
    {
        public string MedicineTitle { get; set; }
        public bool? Morning { get; set; }
        public bool? Noon { get; set; }
        public bool? Evening { get; set; }
    }
}
