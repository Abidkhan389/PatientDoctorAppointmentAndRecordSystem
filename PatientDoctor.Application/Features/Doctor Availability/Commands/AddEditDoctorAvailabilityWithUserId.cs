
namespace PatientDoctor.Application.Features.Doctor_Availability.Commands
{
    public class AddEditDoctorAvailabilityWithUserId : TableParam, IRequest<IResponse>
    {
        public AddEditDoctorAvailabilityCommands AddEditDoctorAvailabilityObj { get; }
        public Guid UserId { get; }

        public AddEditDoctorAvailabilityWithUserId(AddEditDoctorAvailabilityCommands model, Guid Userid)
        {
            AddEditDoctorAvailabilityObj = model;
            this.UserId = Userid;
        }
    }
}
