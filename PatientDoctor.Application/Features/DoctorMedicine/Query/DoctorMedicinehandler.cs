using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.DoctorMedicine.Query;
public class DoctorMedicinehandler: IRequestHandler<DoctorMedicineById, IResponse>
{
    private readonly IDoctorMedicineRepository _doctorMedicineRepository;

    public DoctorMedicinehandler(IDoctorMedicineRepository doctorMedicineRepository)
    {
        _doctorMedicineRepository = doctorMedicineRepository ?? throw new ArgumentNullException(nameof(doctorMedicineRepository));
    }
    public async Task<IResponse> Handle(DoctorMedicineById request, CancellationToken cancellationToken)
    {
        return await _doctorMedicineRepository.GetDoctorMedicineById(request.Medicineid);
    }

}
