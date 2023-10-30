using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetAllRoles : IRequest<IResponse>
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
