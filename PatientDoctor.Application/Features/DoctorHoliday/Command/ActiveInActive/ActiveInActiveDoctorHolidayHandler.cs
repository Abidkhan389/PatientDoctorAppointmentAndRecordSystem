
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
public class ActiveInActiveDoctorHolidayHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<ActiveInActiveDoctorHoliday, IResponse>
{
    private readonly IDoctorHolidayRepository _repo =
        _doctorHolidayRepository ?? throw new ArgumentNullException(nameof(_doctorHolidayRepository));
    public async Task<IResponse> Handle(ActiveInActiveDoctorHoliday request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.ActiveInActive(request);
    }
}

