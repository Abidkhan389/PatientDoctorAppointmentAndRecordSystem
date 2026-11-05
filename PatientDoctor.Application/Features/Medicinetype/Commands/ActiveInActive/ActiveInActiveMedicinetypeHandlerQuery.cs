 
namespace PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive
{
    public class ActiveInActiveMedicinetypeHandlerQuery : IRequestHandler<ActiveInActiveMedicinetype, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public ActiveInActiveMedicinetypeHandlerQuery(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository  ?? throw new ArgumentNullException(nameof(medicinetypeRepository));
        }
        public async Task<IResponse> Handle(ActiveInActiveMedicinetype request, CancellationToken cancellationToken)
        {
            var medicinetype = await _medicinetypeRepository.ActiveInActive(request);
            return medicinetype;
        }
    }
}
