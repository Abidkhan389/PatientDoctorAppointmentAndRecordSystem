
namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserList : TableParam, IRequest<IResponse>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Status { get; set; }
        public string? Email { get; set; }
        public string? Cnic { get; set; }
        public string? MobileNumber { get; set; }
        public string? City { get; set; }
    }
}
