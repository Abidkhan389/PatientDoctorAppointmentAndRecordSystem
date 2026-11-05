
namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
public class GetAllPatientCheckUpHistroyByDoctor : TableParam, IRequest<IResponse>
{
    public GetPatientCheckUpHistryList getPatientHistoryListObj { get; }
    public string UserId { get; }

    public GetAllPatientCheckUpHistroyByDoctor(GetPatientCheckUpHistryList model, Guid Userid)
    {
        getPatientHistoryListObj = model;
        this.UserId = Userid.ToString();
    }
}

