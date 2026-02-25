
namespace PatientDoctor.Application.Features.Patient.Commands.PatientDiscount;
    public class PatientDiscount: IRequest<IResponse>
    {
        public Guid PatientId { get; set; }
        // public Guid PatientIdGuid { get; set; }
        public string DoctorId { get; set; }
        public int DiscountFee { get; set; }
    }

