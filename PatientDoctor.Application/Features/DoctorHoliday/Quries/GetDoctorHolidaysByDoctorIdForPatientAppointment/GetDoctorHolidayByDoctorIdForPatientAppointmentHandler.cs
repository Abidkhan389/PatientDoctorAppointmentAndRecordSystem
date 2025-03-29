using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
public class GetDoctorHolidayByDoctorIdForPatientAppointmentHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<GetDoctorHolidayByDoctorIdForPatientAppointment, IResponse>
{
    public async Task<IResponse> Handle(GetDoctorHolidayByDoctorIdForPatientAppointment request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.GetDoctorHolidayByDoctorIdForPatientAppointment(request);
    }
}

