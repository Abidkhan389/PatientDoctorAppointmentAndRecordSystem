
namespace PatientDoctor.Application.Features.Medicinetype.Quries;
    public class GetMedicineTypeListQueryHandler : IRequestHandler<GetMedicineTypeList, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public GetMedicineTypeListQueryHandler(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository ?? throw new ArgumentNullException(nameof(medicinetypeRepository));
        }
        public async Task<IResponse> Handle(GetMedicineTypeList request, CancellationToken cancellationToken)
        {
            var medicineTypeList = await _medicinetypeRepository.GetAllByProc(request);
            return medicineTypeList;
        }
    }

