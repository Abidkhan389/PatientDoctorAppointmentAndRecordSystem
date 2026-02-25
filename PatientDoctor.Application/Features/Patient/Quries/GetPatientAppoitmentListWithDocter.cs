
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientAppoitmentListWithDocter : TableParam, IRequest<IResponse>
    {
        public GetPatientAppoitmentsList GetPatientAppoitmentsListObj { get; }
        public string DocterId { get; }
        public GetPatientAppoitmentListWithDocter(GetPatientAppoitmentsList model, Guid docterId)
        {
            GetPatientAppoitmentsListObj = model;
            DocterId = docterId.ToString();
        }
    }

