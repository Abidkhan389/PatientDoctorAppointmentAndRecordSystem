using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
using PatientDoctor.Application.Features.Medicinetype.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Repositories.MedicineType
{
    public class MedicineTypeRepository : IMedicinetypeRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IResponse _response;
        private readonly ICountResponse _countResp;
        private readonly ICryptoService _crypto;

        public MedicineTypeRepository(DocterPatiendDbContext context, IResponse response,
            ICountResponse countResp, ICryptoService crypto)
        {
            this._context = context;
            this._response = response;
            this._countResp = countResp;
            this._crypto = crypto;
        }
        public async Task<IResponse> ActiveInActive(ActiveInActiveMedicinetype model)
        {
            try
            {
                var medicinetype = await _context.MedicineType.FindAsync(model.Id);
                if (medicinetype == null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "patient");
                    _response.Success = Constants.ResponseFailure;
                }
                medicinetype.Status = model.Status;
                _context.MedicineType.Update(medicinetype);
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

        public async Task<IResponse> AddEditMedicineType(AddEditMedicineTypeWithUserId model)
        {
            try
            {
                if(model.addEditMedicineTypeObj.MedicineTypeId == Guid.Empty)
                {
                    var medicineTypeObj= await _context.MedicineType.Where(x=> x.TypeName.ToLower()==
                        model.addEditMedicineTypeObj.TypeName.ToLower()).FirstOrDefaultAsync();
                    if(medicineTypeObj != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "{medicineTypeObj.TypeName");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                        PatientDoctor.domain.Entities.MedicineType medicinetype = new PatientDoctor.domain.Entities.MedicineType(model.addEditMedicineTypeObj, model.UserId);
                        await _context.MedicineType.AddAsync(medicinetype);
                        _context.SaveChanges();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataSaved;
                    }
                    return _response;
                }
                else
                {
                    var existMedicineTypeObj= await _context.MedicineType.Where(x=> x.Id== model.addEditMedicineTypeObj.MedicineTypeId).FirstOrDefaultAsync();
                    if(existMedicineTypeObj == null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{MedicineType}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    else if(existMedicineTypeObj.TypeName.ToLower()== model.addEditMedicineTypeObj.TypeName.ToLower())
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "MedicineName");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                        //updating Existing Medicine type
                        existMedicineTypeObj.TypeName = model.addEditMedicineTypeObj.TypeName;
                        _context.MedicineType.Update(existMedicineTypeObj);
                        await _context.SaveChangesAsync();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataUpdate;

                    }
                    return _response;
                }
            }
            catch(Exception ex) 
            {
                _response.Message=ex.Message;
                _response.Success = Constants.ResponseFailure;
                return _response;
            }
        }

        public async Task<IResponse> GetAllByProc(GetMedicineTypeList model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "TypeName" : model.Sort;
            var data= (from medicinetype in _context.MedicineType
                      where(
                                (EF.Functions.ILike(medicinetype.TypeName, $"%{model.TypeName}%") || string.IsNullOrEmpty(model.TypeName))
                      )
                      select new VM_MedicineType
                      {
                          typeName= medicinetype.TypeName,
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

        public async Task<IResponse> GetMedicineTypeById(Guid Id)
        {
            var medicineTypeObj = await (from medicinetype in _context.MedicineType
                                         where (medicinetype.Id == Id)
                                         select new VM_MedicinetypeById
                                         {
                                             Typename= medicinetype.TypeName,
                                         }).FirstOrDefaultAsync();

            if (medicineTypeObj != null)
            {
                _response.Data = medicineTypeObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "MedicineType");
            }
            return _response;
        }
    }
}
