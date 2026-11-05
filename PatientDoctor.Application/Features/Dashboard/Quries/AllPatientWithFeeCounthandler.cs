
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class AllPatientWithFeeCounthandler :IRequestHandler<AllPatientWithFeeCount, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public AllPatientWithFeeCounthandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }

        public Task<IResponse> Handle(AllPatientWithFeeCount request, CancellationToken cancellationToken)
        {
            return _dashboard.GetAllPatientWithFee(request);
        }
    }
}
