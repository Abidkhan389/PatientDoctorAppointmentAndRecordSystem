
namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
public class GetDoctorHolidayListHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<GetDoctorHolidayList, IResponse>
{
    private readonly IDoctorHolidayRepository _repo =
       _doctorHolidayRepository ?? throw new ArgumentNullException(nameof(_doctorHolidayRepository));
    public async Task<IResponse> Handle(GetDoctorHolidayList request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.GetAllByProc(request);
    }
}

