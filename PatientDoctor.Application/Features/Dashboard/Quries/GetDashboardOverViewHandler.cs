
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
