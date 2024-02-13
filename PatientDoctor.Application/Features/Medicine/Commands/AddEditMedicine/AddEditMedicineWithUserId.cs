using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;

namespace PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine
{
    public class AddEditMedicineWithUserId : TableParam, IRequest<IResponse>
    {
        public AddEditMedicineCommand addEditMedicineObj { get; }
        public Guid UserId { get; }
        public AddEditMedicineWithUserId(AddEditMedicineCommand model, Guid UserId)
        {
            addEditMedicineObj = model;
            this.UserId = UserId;
        }
    }
}
