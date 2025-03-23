using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class VM_PreviousDayPatientsRecordCount
    {
        public int PreviousDayPatientChecked { get; set; }
        public int PreviousDayPatientUnChecked { get; set; }
        public int PreviousDayPatientTotal => PreviousDayPatientChecked + PreviousDayPatientUnChecked;
    }
}
