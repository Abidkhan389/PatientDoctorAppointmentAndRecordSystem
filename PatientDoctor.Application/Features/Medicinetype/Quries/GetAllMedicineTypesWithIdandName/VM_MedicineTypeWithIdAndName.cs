﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries.GetAllMedicineTypesWithIdandName
{
    public class VM_MedicineTypeWithIdAndName
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }

    }
}
