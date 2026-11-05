
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class PreviousDayPatientsRecordHandler: IRequestHandler<PreviousDayPatientsRecord, IResponse>
    {
        private readonly IDashboardRepository _dashboard;
        public PreviousDayPatientsRecordHandler(IDashboardRepository dashboard)
        {
            this._dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
        }
        public Task<IResponse> Handle(PreviousDayPatientsRecord request, CancellationToken cancellationToken)
        {
            return _dashboard.GetPreviousDayPatientsRecord(request);
        }
    }
}
