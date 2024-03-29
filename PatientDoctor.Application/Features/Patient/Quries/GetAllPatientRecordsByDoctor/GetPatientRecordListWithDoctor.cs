﻿using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor
{
    public class GetPatientRecordListWithDoctor : TableParam, IRequest<IResponse>
    {
        public GetPatientRecordList getPatientRecordList { get; }
        public string DocterId { get; }
        public GetPatientRecordListWithDoctor(GetPatientRecordList model, Guid docterId)
        {
            getPatientRecordList = model;
            DocterId = docterId.ToString();
        }
    }
}
