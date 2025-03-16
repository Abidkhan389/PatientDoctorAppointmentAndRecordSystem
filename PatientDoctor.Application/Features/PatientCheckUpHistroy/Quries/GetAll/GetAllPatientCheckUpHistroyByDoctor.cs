using MediatR;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;

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

