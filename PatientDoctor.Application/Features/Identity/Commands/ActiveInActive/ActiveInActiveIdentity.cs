﻿using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Identity.Commands.ActiveInActive
{
    public class ActiveInActiveIdentity : IRequest<IResponse>
    {
        public string Id { get; set; }
        public int Status { get; set; }

    }
}
