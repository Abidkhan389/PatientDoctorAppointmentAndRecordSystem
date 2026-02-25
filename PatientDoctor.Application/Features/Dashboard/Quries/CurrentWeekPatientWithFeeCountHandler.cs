
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class CurrentWeekPatientWithFeeCountHandler : IRequestHandler<CurrentWeekPatientWithFeeCount, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public CurrentWeekPatientWithFeeCountHandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }

        public Task<IResponse> Handle(CurrentWeekPatientWithFeeCount request, CancellationToken cancellationToken)
        {
            return _dashboard.GetAllPatientWithFeeCurrentWeek(request);
        }
    }
}
