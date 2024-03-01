using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetById;
using PatientDoctor.Application.Features.Medicine.Quries.GetById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;

namespace PatientDoctor.Infrastructure.Repositories.DoctorFeeCheckUpFee
{
	public class DoctorCheckUpFeeRepository : IDoctorCheckUpFeeRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IResponse _response;
        private readonly ICountResponse _countResp;
        private readonly ICryptoService _crypto;

        public DoctorCheckUpFeeRepository(DocterPatiendDbContext context, IResponse response,
            ICountResponse countResp, ICryptoService crypto)
        {
            _context = context;
            _response = response;
            _countResp = countResp;
            _crypto = crypto;
        }

        public async Task<IResponse> ActiveInActive(ActiveInActiveDoctorCheckupFee model)
        {
            try
            {
                var doctorCheckUpFeeObj = await _context.DoctorCheckUpFeeDetails.FindAsync(model.Id);
                if(doctorCheckUpFeeObj == null)
                {
                    _response.Message = Constants.NotFound.Replace("{data}", "DoctorFeeCheckUp");
                    _response.Success = Constants.ResponseFailure;
                }
                doctorCheckUpFeeObj.Status = model.Status;
                _context.DoctorCheckUpFeeDetails.Update(doctorCheckUpFeeObj);
                await _context.SaveChangesAsync();
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
            catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
            }
            return _response;
        }

        public async Task<IResponse> AddEditDoctorCheckUpFee(DoctorCheckUpFeeWithUserId model)
        {
            try
            {
                if (model.addEditDoctorCheckUpFee.Id == null)
                {
                    var doctorCheckUpFeeObj = await _context.DoctorCheckUpFeeDetails.Where(x => x.Status != 0 && x.DoctorId ==model.addEditDoctorCheckUpFee.DoctorId).FirstOrDefaultAsync();
                    if (doctorCheckUpFeeObj != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "Doctor Fee");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                         var doctorCheckUpFee = new PatientDoctor.domain.Entities.DoctorCheckUpFeeDetails(model.addEditDoctorCheckUpFee, model.UserId);
                        await _context.DoctorCheckUpFeeDetails.AddAsync(doctorCheckUpFee);
                        _context.SaveChanges();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataSaved;
                    }
                }
                else
                {
                    var existdoctorCheckUpFeeObj = await _context.DoctorCheckUpFeeDetails.Where(x => x.Id == model.addEditDoctorCheckUpFee.Id).FirstOrDefaultAsync();
                    if (existdoctorCheckUpFeeObj == null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{existdoctorCheckUpFeeObj}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    else if (await _context.DoctorCheckUpFeeDetails
                             .Where(x => x.Id != model.addEditDoctorCheckUpFee.Id &&
                                    x.DoctorId == model.addEditDoctorCheckUpFee.DoctorId)
                             .FirstOrDefaultAsync() != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "existdoctorCheckUpFeeObj");
                        _response.Success = Constants.ResponseFailure;
                    }
                    else
                    {
                        //updating Existing Medicine type
                        existdoctorCheckUpFeeObj.DoctorFee = model.addEditDoctorCheckUpFee.DoctorFee;
                        existdoctorCheckUpFeeObj.DoctorId = model.addEditDoctorCheckUpFee.DoctorId;
                        existdoctorCheckUpFeeObj.UpdatedBy = model.UserId;
                        existdoctorCheckUpFeeObj.UpdatedOn = DateTime.UtcNow;
                        _context.DoctorCheckUpFeeDetails.Update(existdoctorCheckUpFeeObj);
                        await _context.SaveChangesAsync();
                        _response.Success = Constants.ResponseSuccess;
                        _response.Message = Constants.DataUpdate;
                    }
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
            }
            return _response;
        }

        public async Task<IResponse> GetAllByProc(GetDoctorCheckUpFeeDetailsList model)
        {
            model.Sort = model.Sort == null || model.Sort == "" ? "DoctorName" : model.Sort;
            var data = (from doccheckupfee in _context.DoctorCheckUpFeeDetails
                        join doctor in _context.Users on doccheckupfee.DoctorId equals doctor.Id
                        where (
                                (EF.Functions.ILike(doctor.UserName, $"%{model.DoctorName}%") || string.IsNullOrEmpty(model.DoctorName))
                              && (doccheckupfee.DoctorFee == model.DoctorFee || model.DoctorFee == null)
                             )
                        select new VM_DoctorCheckUpFeeDetails
                        {
                            Id= doccheckupfee.Id,
                            DocotrId= doccheckupfee.DoctorId,
                            DoctorName= doctor.UserName,
                            DocterFee=doccheckupfee.DoctorFee,
                            Status=doccheckupfee.Status
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

        public async Task<IResponse> GetDoctorCheckUpFeeById(Guid Id)
        {
            var doctorCheckUpFeeObj = await (from docckupfee in _context.DoctorCheckUpFeeDetails
                                    where (docckupfee.Id == Id)
                                    select new VM_DocterCheckUpFeeDetails
                                    {
                                        DocterId = docckupfee.DoctorId,
                                        DocterFee = docckupfee.DoctorFee
                                    }).FirstOrDefaultAsync();

            if (doctorCheckUpFeeObj != null)
            {
                _response.Data = doctorCheckUpFeeObj;
                _response.Message = Constants.GetData;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "doctorCheckUpFeeObj");
            }
            return _response;
        }
    }
}

