using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Helpers;
using System;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
public class GetByIdDoctorHolidayHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<GetByIdDoctorHoliday, IResponse>
{
    private readonly IDoctorHolidayRepository _repo =
        _doctorHolidayRepository ?? throw new ArgumentNullException(nameof(_doctorHolidayRepository));
    public async Task<IResponse> Handle(GetByIdDoctorHoliday request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.GetByIdDoctorHoliday(request);
    }
}

