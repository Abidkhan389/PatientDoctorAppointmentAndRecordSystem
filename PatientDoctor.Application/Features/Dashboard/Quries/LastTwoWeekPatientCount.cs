
namespace PatientDoctor.Application.Features.Dashboard.Quries
{
    public class LastTwoWeekPatientCount : IRequest<IResponse>
    {
        public string logInUserId { get; set; }
        public string logInUserRole { get; set; }
    }
}
