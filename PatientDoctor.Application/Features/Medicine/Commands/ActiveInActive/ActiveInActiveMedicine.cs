using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive
{
    public class ActiveInActiveMedicine : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
