using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserById : IRequest<IResponse>
    {
        public Guid id { get; set; }
        //public GetUserById(Guid ProductId)
        //{
        //    this.id = ProductId;
        //}
    }
}
