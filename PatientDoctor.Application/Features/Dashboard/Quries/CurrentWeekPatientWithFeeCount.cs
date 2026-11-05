
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class CurrentWeekPatientWithFeeCount :IRequest<IResponse>
    {
        public string logInUserId { get; set; }
        public string logInUserRole { get; set; }
    }
}
