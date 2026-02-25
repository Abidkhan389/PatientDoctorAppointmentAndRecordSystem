 
namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicine;
public class GetAllMedicines : IRequest<IResponse>
{
    public string DoctorId { get; set; }
}

