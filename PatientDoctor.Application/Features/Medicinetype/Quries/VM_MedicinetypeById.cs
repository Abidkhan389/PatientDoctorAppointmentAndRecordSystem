﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries
{
    public class VM_MedicinetypeById
    {
        public string TypeName { get; set; }
        public string? tabletMg { get; set; }
        public List<string>? MedicinePotency { get; set; } = new List<string>(); // Optional

    }
}
