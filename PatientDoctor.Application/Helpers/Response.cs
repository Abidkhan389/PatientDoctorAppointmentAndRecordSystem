﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Helpers
{
    public class Response : IResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class CountResponse : ICountResponse
    {
        public int TotalCount { get; set; }
        public object DataList { get; set; }
    }
}
