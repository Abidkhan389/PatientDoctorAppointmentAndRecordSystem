using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries
{
    public class GetMedicineTypeList : TableParam, IRequest<IResponse>
    {
        public string? TypeName { get; set; }

    }
}
