using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Medicine.Quries.GetById;
using PatientDoctor.Application.Features.Medicinetype.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        _context.SaveChanges();
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
                        existMedicineObj.StartingDate = model.addEditMedicineObj.StartingDate;
                        existMedicineObj.ExperiyDate = model.addEditMedicineObj.ExperiyDate;
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
                        where (
                                  (EF.Functions.ILike(medicine.MedicineName, $"%{model.MedicineName}%") || string.IsNullOrEmpty(model.MedicineName))
                                  && (medicine.DoctorId == model.DoctorId || string.IsNullOrEmpty(model.DoctorId))
                                  && (model.MedicineTypeId == null || medicine.MedicineTypeId == model.MedicineTypeId)
                              )
                        select new VM_Medicine
                        {
                            Id = medicine.Id,
                            MedicineName = medicine.MedicineName,
                            MedicineTypeName = m_type.TypeName,
                            DoctorName = doctor.UserName,
                            Status = medicine.Status,
                            StartingDate = medicine.StartingDate,
                            ExpiryDate = medicine.ExperiyDate // corrected from ExperiyDate to ExpiryDate
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
                                         where (medicine.Id == Id)
                                         select new VM_MedicineById
                                         {
                                             MedicineName = medicine.MedicineName,
                                             MedicineTypeId = medicine.MedicineTypeId,
                                             DoctorId = medicine.DoctorId,
                                             StartingDate = medicine.StartingDate,
                                             ExpiryDate=medicine.ExperiyDate
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
    }
}
