
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class VM_PreviousDayPatientsRecordCount
    {
        public int PreviousDayPatientChecked { get; set; }
        public int PreviousDayPatientUnChecked { get; set; }
        public int PreviousDayPatientTotal => PreviousDayPatientChecked + PreviousDayPatientUnChecked;
    }
}
