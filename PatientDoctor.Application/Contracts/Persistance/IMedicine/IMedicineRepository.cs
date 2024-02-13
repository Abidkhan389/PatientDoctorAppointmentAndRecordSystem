using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.IMedicine
{
    public interface IMedicineRepository
    {
        Task<IResponse> GetMedicineById(Guid Id);
        Task<IResponse> AddEditMedicine(AddEditMedicineWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActiveMedicine model);
        Task<IResponse> GetAllByProc(GetMedicineList model);
    }
}
