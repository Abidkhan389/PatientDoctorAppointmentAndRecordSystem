
namespace PatientDoctor.Application.Features.Medicinetype.Quries;
    public class GetMedicineTypeList : TableParam, IRequest<IResponse>
    {
        public string? TypeName { get; set; }

    }

