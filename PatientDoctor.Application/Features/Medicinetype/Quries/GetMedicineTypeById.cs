using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries
{
    public class GetMedicineTypeById : IRequest<IResponse>
    {
        public Guid Id { get; set; }

    }
}
