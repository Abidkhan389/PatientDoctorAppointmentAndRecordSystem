using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    public class AddEditMedicinetypeCommandHandler : IRequestHandler<AddEditMedicineTypeWithUserId, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public AddEditMedicinetypeCommandHandler(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository  ?? throw new ArgumentNullException(nameof(medicinetypeRepository));
        }
        public async Task<IResponse> Handle(AddEditMedicineTypeWithUserId request, CancellationToken cancellationToken)
        {
            var medicinetype= await _medicinetypeRepository.AddEditMedicineType(request);
            return medicinetype;
        }
    }
}
