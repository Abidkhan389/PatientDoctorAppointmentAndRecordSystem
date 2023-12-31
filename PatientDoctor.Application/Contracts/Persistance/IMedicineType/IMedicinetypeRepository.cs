using PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
using PatientDoctor.Application.Features.Medicinetype.Quries;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.IMedicineType
{
    public interface IMedicinetypeRepository
    {
        Task<IResponse> GetMedicineTypeById(Guid Id);
        Task<IResponse> AddEditMedicineType(AddEditMedicineTypeWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActiveMedicinetype model);
        Task<IResponse> GetAllByProc(GetMedicineTypeList model);
    }
}
