
namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserById : IRequest<IResponse>
    {
        public string id { get; set; }
        //public GetUserById(Guid ProductId)
        //{
        //    this.id = ProductId;
        //}
    }
}
