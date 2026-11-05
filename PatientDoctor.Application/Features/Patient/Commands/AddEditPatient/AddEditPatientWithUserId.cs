
namespace PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
    public class AddEditPatientWithUserId:TableParam, IRequest<IResponse>
    {
        public AddEditPatientCommand AddEditPatientObj { get; }
        public Guid UserId { get; }

        public AddEditPatientWithUserId(AddEditPatientCommand model, Guid Userid)
        {
            AddEditPatientObj = model;
            this.UserId = Userid;
        }
    }

