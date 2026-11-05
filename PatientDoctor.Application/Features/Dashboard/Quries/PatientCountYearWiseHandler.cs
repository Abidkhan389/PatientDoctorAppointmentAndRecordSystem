
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    internal class PatientCountYearWiseHandler : IRequestHandler<PatientCountYearWise, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public PatientCountYearWiseHandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }
        public Task<IResponse> Handle(PatientCountYearWise request, CancellationToken cancellationToken)
        {
            return _dashboard.GetPatientCountYearlyWise(request);
        }
    }
}
