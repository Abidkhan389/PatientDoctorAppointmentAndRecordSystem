using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Dashboard;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class GetDashboardOverViewHandler : IRequestHandler<DashboardOverView, IResponse>
    {
        private readonly IDashboardRepository _dashboard;

        public GetDashboardOverViewHandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }
        public Task<IResponse> Handle(DashboardOverView request, CancellationToken cancellationToken)
        {
            return _dashboard.GetOverViewForAdminDashboard();
        }
    }
}
