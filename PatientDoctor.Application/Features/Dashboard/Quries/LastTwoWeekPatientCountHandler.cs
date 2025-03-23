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
    public class LastTwoWeekPatientCountHandler : IRequestHandler<LastTwoWeekPatientCount, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public LastTwoWeekPatientCountHandler(IDashboardRepository dashboard)
        {
            _dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard)); ;
        }
        public Task<IResponse> Handle(LastTwoWeekPatientCount request, CancellationToken cancellationToken)
        {
            return _dashboard.GetLastTwoWeekPatientRecord(request);
        }
    }
}
