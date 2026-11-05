
namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
    public class AddEditMedicineTypeWithUserId : TableParam, IRequest<IResponse>
    {
        public AddEditMedicineTypeCommand addEditMedicineTypeObj { get; }
        public Guid UserId { get; }
        public AddEditMedicineTypeWithUserId(AddEditMedicineTypeCommand model, Guid Userid)
        {
            addEditMedicineTypeObj = model;
            this.UserId = Userid;
        }
    }

