
using MediatR;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Quries;
public class GetPatientListWithUser : TableParam, IRequest<IResponse> 
{
    public GetPatientList getPatientListObj { get; }
    public string UserId { get; }

    public GetPatientListWithUser(GetPatientList model, Guid Userid)
    {
        getPatientListObj = model;
        this.UserId = Userid.ToString(); ;
    }
}

