 
namespace PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine
{
    public class AddEditMedicineHandler : IRequestHandler<AddEditMedicineWithUserId, IResponse>
    {
        private readonly IMedicineRepository _medicineRepository;

        public AddEditMedicineHandler(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }
        public async Task<IResponse> Handle(AddEditMedicineWithUserId request, CancellationToken cancellationToken)
        {
            return await _medicineRepository.AddEditMedicine(request);
        }
    }
}
