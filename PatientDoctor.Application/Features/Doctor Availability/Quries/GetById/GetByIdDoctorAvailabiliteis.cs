using MediatR;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetById;
    public class GetByIdDoctorAvailabiliteis : IRequest<IResponse>
    {
        public Guid Id { get; set; }
    }

