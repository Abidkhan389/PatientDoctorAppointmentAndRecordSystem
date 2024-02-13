using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetById
{
    public class GetMedicineById : IRequest<IResponse>
    {
        public Guid Id { get; set; }
    }
}
