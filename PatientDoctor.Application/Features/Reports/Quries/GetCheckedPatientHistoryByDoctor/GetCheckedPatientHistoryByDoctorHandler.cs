
namespace PatientDoctor.Application.Features.Reports.Quries.GetCheckedPatientHistoryByDoctor;
public class GetCheckedPatientHistoryByDoctorHandler : IRequestHandler<GetCheckedPatientHistoryByDoctorQuery, IResponse>
{
    private readonly IReports _reports;

    public GetCheckedPatientHistoryByDoctorHandler(IReports reports)
    {
        _reports = reports ?? throw new ArgumentNullException(nameof(reports));
    }
    public async Task<IResponse> Handle(GetCheckedPatientHistoryByDoctorQuery request, CancellationToken cancellationToken)
    {
        var reports=await  _reports.GetCheckedPatientHistoryByDoctor(request);
        if (reports.Data == null)
            throw new UnauthorizedException($"User is not Authorized For Reports.");
        return reports;
    }
}

