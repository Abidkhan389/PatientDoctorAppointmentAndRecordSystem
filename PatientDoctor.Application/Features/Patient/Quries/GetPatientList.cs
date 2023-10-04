﻿using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class GetPatientList 
    {
        public string? PatientName { get; set; }
        public int? ActiveStatus { get; set; }
        public string? BloodType { get; set; }
        public string? Cnic { get; set; }
        public string? MobileNumber { get; set; }
        public string? City { get; set; }
    }
}
