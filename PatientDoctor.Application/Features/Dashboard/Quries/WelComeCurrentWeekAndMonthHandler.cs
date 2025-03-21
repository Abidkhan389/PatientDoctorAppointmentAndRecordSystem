using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Dashboard;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class WelComeCurrentWeekAndMonthHandler : IRequestHandler<WelComeCurrentWeekAndMonth, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public WelComeCurrentWeekAndMonthHandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }
        public Task<IResponse> Handle(WelComeCurrentWeekAndMonth request, CancellationToken cancellationToken)
        {
            return _dashboard.GetPatientCurrentWeekAndMonth(request);
        }
    }
}
