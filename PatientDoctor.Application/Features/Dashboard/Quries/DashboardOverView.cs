using MediatR;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class DashboardOverView: IRequest<IResponse>
    {
        public int LastTwoMonthsDoctorCount { get; set; }
        public int DoctorCount { get; set; }
        public int LastTwoMonthsAdminCount { get; set; }
        public int AdminCount { get; set; }
        public int LastTwoMonthsPatientCount { get; set; }
        public int PatientCount { get; set; }
        public int LastTwoMotnhsReceptionistCount { get; set; }
        public int RceptionistCount { get; set; }
        public List<AdmindashBoardDoctorOverView> PatientPerDoctorCount { get; set; }
    }
    public class AdmindashBoardDoctorOverView
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int PatientCount { get; set; }

    }
    
}
