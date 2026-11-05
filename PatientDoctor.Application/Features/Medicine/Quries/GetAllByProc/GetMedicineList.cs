 
namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc
{
    public class GetMedicineList : TableParam, IRequest<IResponse>
    {
        public string? MedicineName { get; set; }
    }
}
