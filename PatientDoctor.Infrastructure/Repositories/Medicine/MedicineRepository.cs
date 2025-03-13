using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicineTypes;
using PatientDoctor.Application.Features.Medicine.Quries.GetById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;

namespace PatientDoctor.Infrastructure.Repositories.Medicine
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IResponse _response;
        private readonly ICountResponse _countResp;
        private readonly ICryptoService _crypto;

        public MedicineRepository(DocterPatiendDbContext context, IResponse response,
            ICountResponse countResp, ICryptoService crypto)
        {
            this._context = context;
            this._response = response;
            this._countResp = countResp;
            this._crypto = crypto;
        }
        public async Task<IResponse> ActiveInActive(ActiveInActiveMedicine model)
        {
            try
            {
                var medicine = await _context.Medicine.FindAsync(model.Id);
                if (medicine == null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "Medicine");
                    _response.Success = Constants.ResponseFailure;
                }
                 medicine.Status = model.Status;
                _context.Medicine.Update(medicine);
                await _context.SaveChangesAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
            }
            return _response;
        }

        public async Task<IResponse> AddEditMedicine(AddEditMedicineWithUserId model)
        {
            try
            {
                if(model.addEditMedicineObj.Id == null)
                {
                    var medicineObj = await _context.Medicine.Where(x => x.Status != 0 && x.MedicineName.ToLower() ==
                                    model.addEditMedicineObj.MedicineName.ToLower()).FirstOrDefaultAsync();
                    if (medicineObj != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "medicine Name");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                        PatientDoctor.domain.Entities.Medicine medicine = new PatientDoctor.domain.Entities.Medicine(model.addEditMedicineObj, model.UserId);
                        await _context.Medicine.AddAsync(medicine);
                        await _context.SaveChangesAsync();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataSaved;
                    }
                }
                else
                {
                    var existMedicineObj = await _context.Medicine.Where(x => x.Id == model.addEditMedicineObj.Id).FirstOrDefaultAsync();
                    if (existMedicineObj == null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{MedicineType}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    else if (await _context.Medicine
                             .Where(x => x.Id != model.addEditMedicineObj.Id &&
                                    x.MedicineName == model.addEditMedicineObj.MedicineName)
                             .FirstOrDefaultAsync() != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "MedicineName");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                        //updating Existing Medicine type
                        existMedicineObj.MedicineName = model.addEditMedicineObj.MedicineName;
                        existMedicineObj.DoctorId = model.addEditMedicineObj.DoctorId;
                        existMedicineObj.MedicineTypeId = model.addEditMedicineObj.MedicineTypeId;
                        existMedicineObj.medicineTypePotencyId = model.addEditMedicineObj.MedicineTypePotencyId;
                        existMedicineObj.StartingDate = model.addEditMedicineObj.StartingDate;
                        existMedicineObj.ExpiryDate = model.addEditMedicineObj.ExpiryDate;
                        existMedicineObj.UpdatedBy = model.UserId;
                        existMedicineObj.UpdatedOn = DateTime.UtcNow;
                        _context.Medicine.Update(existMedicineObj);
                        await _context.SaveChangesAsync();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataUpdate;

                    }
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
                return _response;
            }
        }

        public async Task<IResponse> GetAllByProc(GetMedicineList model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "MedicineName" : model.Sort;
            var data = (from medicine in _context.Medicine
                        join doctor in _context.Users on medicine.DoctorId equals doctor.Id
                        join m_type in _context.MedicineType on medicine.MedicineTypeId equals m_type.Id
                        join m_type_potency in _context.MedicinePotency on medicine.medicineTypePotencyId equals m_type_potency.Id
                        where (
                            (string.IsNullOrEmpty(model.MedicineName) ||
                             EF.Functions.Like(medicine.MedicineName, $"%{model.MedicineName}%"))
                        )
                        select new VM_Medicine
                        {
                            Id = medicine.Id,
                            MedicineName = medicine.MedicineName,
                            MedicineTypeName = m_type.TypeName,
                            MedicineTypePotencyName = m_type_potency.Potency,
                            DoctorName = doctor.UserName,
                            Status = medicine.Status,
                            StartingDate = medicine.StartingDate,
                            ExpiryDate = medicine.ExpiryDate // Ensure the property is correctly spelled
                        }).AsQueryable();


            var count = data.Count();
            var sorted = await HelperStatic.OrderBy(data, model.SortEx, model.OrderEx == "desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = _countResp;
            return _response;
        }

        public async Task<IResponse> GetMedicineById(Guid Id)
        {
            var medicineObj = await (from medicine in _context.Medicine
                                     join user in _context.Users on medicine.DoctorId equals user.Id
                                     join medicineType in _context.MedicineType on medicine.MedicineTypeId equals medicineType.Id
                                     join medicinePotency in _context.MedicinePotency on medicine.medicineTypePotencyId equals medicinePotency.Id
                                     where (medicine.Id == Id)
                                         select new VM_MedicineById
                                         {
                                             MedicineName = medicine.MedicineName,
                                             MedicineTypeName = medicineType.TypeName,
                                             MedicineTypeId = medicineType.Id,
                                             MedicineTypePotencyName = medicinePotency.Potency,
                                             MedicineTypePotencyId = medicinePotency.Id,
                                             DoctorId = user.Id,
                                             DoctorName = user.UserName,
                                             StartingDate = medicine.StartingDate,
                                             ExpiryDate=medicine.ExpiryDate
                                         }).FirstOrDefaultAsync();
            if (medicineObj != null)
            {
                _response.Data = medicineObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Medicine");
            }
            return _response;
        }

        public async Task<IResponse> GetAllMedicineTypePotency(Guid Id)
        {
            var medicineTypePotencyObj = await (from medicinepotency in _context.MedicinePotency
                                                where (medicinepotency.MedicineTypeId == Id)
                                                select new VM_MedicineTypePotency
                                                {
                                                    MedicineTypePotencyId = medicinepotency.Id,
                                                    Potency = medicinepotency.Potency,
                                                    MedicineTypeId = medicinepotency.MedicineTypeId
                                                }).ToListAsync();

            if (medicineTypePotencyObj != null)
            {
                _response.Data = medicineTypePotencyObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Medicine Potency");
            }
            return _response;
        }

        public async Task<IResponse> GetAllMedicineTypeList()
        {
            var medicineTypeObj = await (from medicinetype in _context.MedicineType
                                         select new VM_GetAllMedicineTypes
                                         {
                                             MedicineTypeId = medicinetype.Id,
                                             MedicineTypeName = medicinetype.TypeName
                                         }).ToListAsync();

            if (medicineTypeObj != null)
            {
                _response.Data = medicineTypeObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Medicine Type List");
            }
            return _response;
        }
        public async Task<IResponse> GetAllMedicineList(GetAllMedicines model)
        {
            var medicineTypeObj = await (from medicine in _context.Medicine
                                         where medicine.DoctorId == model.DoctorId
                                         select new VM_AllMedicine
                                         {
                                             MedicineId = medicine.Id,
                                             MedicineName = medicine.MedicineName
                                         }).ToListAsync();

            if (medicineTypeObj != null)
            {
                _response.Data = medicineTypeObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "Medicine  List");
            }
            return _response;
        }
    }
}
