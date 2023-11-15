using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.Dashboard
{
    public interface IDashboardRepository
    {
        Task<IResponse> GetOverViewForAdminDashboard();
    }
}
