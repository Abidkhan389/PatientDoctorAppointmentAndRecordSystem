
namespace PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
public class AddEditDoctorHolidayHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<AddEditDoctorHolidayCommand, IResponse>
{
    private readonly IDoctorHolidayRepository _repo =
        _doctorHolidayRepository ?? throw new ArgumentNullException(nameof(_doctorHolidayRepository));
    public async Task<IResponse> Handle(AddEditDoctorHolidayCommand request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.AddEditDoctorHoliday(request);
    }
}

