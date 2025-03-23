using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class VM_LastTwoeWeekPatientCount
    {
        public int LastWeekPatientCount { get; set; }
        public int SecondLastWeekPatientCount { get; set; }
    }
}
