using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.Dashboard;
using PatientDoctor.Application.Features.Dashboard.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICountResponse countResp;

        public DashboardRepository(DocterPatiendDbContext context,
            IResponse response, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ICountResponse countResp)
        {
            this._context = context;
            this._response = response;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.countResp = countResp;
        }

        public async Task<IResponse> GetOverViewForAdminDashboard()
        {
            DashboardOverView dashboardOverView = new();
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);

            var PatientDetails = await _context.PatientDetails.Where(x => x.Status == 1).ToListAsync();
            //var users = await _context.Users.Where(x => x.Status == 1 && (x.RoleName == "Admin" || x.RoleName == "Doctor")).ToListAsync();
            var users = await _context.Users.Where(x => x.Status == 1).ToListAsync();

            // Fetch UserIds for the selected users
            var userIds = users.Select(u => u.Id).Distinct().ToList();

            var UsersDetails = await _context.Userdetail.Where(ud => userIds.Contains(ud.UserId)).ToListAsync();

            var UsersAddedLastTwoMonths = UsersDetails.Where(ud => ud.CreatedOn >= twoMonthsAgo).ToList();
            // Calculating the patient count per doctor 
            var patientCountByDoctor = await _context.Users
                .Where(x => x.Status == 1 && x.RoleName == "Doctor")
                .GroupBy(x => x.Id)
                .Select(group => new AdmindashBoardDoctorOverView
                {
                    DoctorId = group.Key,
                    DoctorName = group.First().UserName, // Optional: Include the doctor's username
                    PatientCount = _context.Patient.Count(patient => patient.DoctoerId == group.Key)
                })
                .ToListAsync();

            dashboardOverView.PatientPerDoctorCount = patientCountByDoctor;
            dashboardOverView.PatientCount = PatientDetails.Count();
            dashboardOverView.AdminCount = users.Count(x => x.RoleName == "Admin");
            dashboardOverView.DoctorCount = users.Count(x => x.RoleName == "Doctor");
            dashboardOverView.RceptionistCount = users.Count(x => x.RoleName == "Receptionist");
            dashboardOverView.LastTwoMonthsPatientCount = HelperStatic.PatientCountDateforLastTwoMonths(PatientDetails);
            dashboardOverView.LastTwoMonthsAdminCount = UsersAddedLastTwoMonths.Count(ud => users.Any(u => u.Id == ud.UserId && u.RoleName == "Admin"));
            dashboardOverView.LastTwoMonthsDoctorCount = UsersAddedLastTwoMonths.Count(ud => users.Any(u => u.Id == ud.UserId && u.RoleName == "Doctor"));
            dashboardOverView.LastTwoMotnhsReceptionistCount = UsersAddedLastTwoMonths.Count(ud => users.Any(u => u.Id == ud.UserId && u.RoleName == "Receptionist"));
            _response.Data = dashboardOverView;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;

        }

        public async Task<IResponse> GetPatientCurrentWeekAndMonth(WelComeCurrentWeekAndMonth model)
        {
            VM_CurrentWeekAndMonthCount vM_CurrentWeekAndMonth = new VM_CurrentWeekAndMonthCount();
            var today = DateTime.Now.Date.AddDays(1);
            var monthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var currentWeekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            if (model.logInUserRole == "Doctor")
            {
                var CurrentMonthparient = _context.Prescriptions.Where(x => x.DoctorId == model.logInUserId && (x.CreatedOn >= monthFirstDay && x.CreatedOn <= DateTime.Now.Date));
                var CurrentWeekparient = CurrentMonthparient.Where(x => x.DoctorId == model.logInUserId && (x.CreatedOn >= currentWeekStart && x.CreatedOn <= DateTime.Now.Date.AddDays(1)));

                vM_CurrentWeekAndMonth.CurrentMonthPatientCount = CurrentMonthparient.Count();
                vM_CurrentWeekAndMonth.CurrentWeekPatientCount = CurrentWeekparient.Count();
            }
            else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
            {
                var CurrentMonthparient = _context.Prescriptions.Where(x =>x.CreatedOn >= monthFirstDay && x.CreatedOn <= DateTime.Now.Date);
                var CurrentWeekparient = CurrentMonthparient.Where(x =>x.CreatedOn >= currentWeekStart && x.CreatedOn <= DateTime.Now.Date.AddDays(1));
                vM_CurrentWeekAndMonth.CurrentMonthPatientCount = CurrentMonthparient.Count();
                vM_CurrentWeekAndMonth.CurrentWeekPatientCount = CurrentWeekparient.Count();
            }
            _response.Data = vM_CurrentWeekAndMonth;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;
        }

        public async Task<IResponse> GetAllPatientWithFee(AllPatientWithFeeCount model)
        {
            try
            {
                VM_AllPatientWithFeeCount vM_AllPatientWithFee = new VM_AllPatientWithFeeCount();
                if (model.logInUserRole == "Doctor")
                {
                    var totalPatient = await (from ap in _context.Appointment
                                              join p in _context.Patient on ap.PatientId equals p.PatientId
                                              join pr in _context.Prescriptions on ap.PatientId equals pr.PatientId
                                              where pr.DoctorId == model.logInUserId
                                              select new
                                              {
                                                  ap.PatientId,
                                                  ap.DoctorFee
                                              }).ToListAsync();

                    var allpatientDoctorFee = totalPatient.Sum(x => x.DoctorFee);

                    vM_AllPatientWithFee.PatientCount = totalPatient.Count();
                    vM_AllPatientWithFee.PatientFeeCount = allpatientDoctorFee;
                }
                else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
                {
                    var totalPatient = await (from ap in _context.Appointment
                                              join p in _context.Patient on ap.PatientId equals p.PatientId
                                              join pr in _context.Prescriptions on ap.PatientId equals pr.PatientId
                                              select new
                                              {
                                                  ap.PatientId,
                                                  ap.DoctorFee
                                              }).ToListAsync();

                    var allpatientDoctorFee = totalPatient.Sum(x => x.DoctorFee);

                    vM_AllPatientWithFee.PatientCount = totalPatient.Count();
                    vM_AllPatientWithFee.PatientFeeCount = allpatientDoctorFee;
                }
                _response.Data = vM_AllPatientWithFee;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ErrorWhileGettingData;
                return _response;
            }
        }

        public async Task<IResponse> GetPatientCountYearlyWise(PatientCountYearWise model)
        {
            try
            {

                List<VM_PatientCountYearWise> revenueForCastDtos = new List<VM_PatientCountYearWise>();
                if (model.logInUserRole == "Doctor")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DateTime firstDayOfCurrentYear = new DateTime(DateTime.Now.AddYears(-i).Year, 1, 1);
                        DateTime lastDayOfCurrentYear = new DateTime(firstDayOfCurrentYear.Year, 12, 31);
                        var yearPayout = _context.Prescriptions
                                                    .Where(s => s.DoctorId == model.logInUserId && s.CreatedOn >= firstDayOfCurrentYear && s.CreatedOn <= lastDayOfCurrentYear)
                                                    .GroupBy(s => s.CreatedOn.Value.Month);

                        VM_PatientCountYearWise VM_PatientCountYearWise = new VM_PatientCountYearWise
                        {
                            Name = firstDayOfCurrentYear.Year.ToString(),
                            Data = GetMonthlyData(yearPayout)
                        };
                        revenueForCastDtos.Add(VM_PatientCountYearWise);
                    }

                    VM_PatientCountYearWise forecastDto = new VM_PatientCountYearWise
                    {
                        Name = DateTime.Now.AddYears(1).Year.ToString(),
                        Data = new List<decimal>()
                    };

                }
                else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DateTime firstDayOfCurrentYear = new DateTime(DateTime.Now.AddYears(-i).Year, 1, 1);
                        DateTime lastDayOfCurrentYear = new DateTime(firstDayOfCurrentYear.Year, 12, 31);
                        var yearPayout = _context.Prescriptions
                                                    .Where(s => s.CreatedOn >= firstDayOfCurrentYear && s.CreatedOn <= lastDayOfCurrentYear)
                                                    .GroupBy(s => s.CreatedOn.Value.Month);

                        VM_PatientCountYearWise VM_PatientCountYearWise = new VM_PatientCountYearWise
                        {
                            Name = firstDayOfCurrentYear.Year.ToString(),
                            Data = GetMonthlyData(yearPayout)
                        };
                        revenueForCastDtos.Add(VM_PatientCountYearWise);
                    }
                }

                _response.Data = revenueForCastDtos;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ErrorWhileGettingData;
                return _response;
            }
        }

        public async Task<IResponse> GetPreviousDayPatientsRecord(PreviousDayPatientsRecord model)
        {
            try
            {
                VM_PreviousDayPatientsRecordCount vM_PreviousDayPatientsRecordCount = new VM_PreviousDayPatientsRecordCount();
                var yesterday = DateTime.Now.Date.AddDays(-1);
                var startOfYesterday = yesterday;
                var endOfYesterday = yesterday.AddDays(1).AddSeconds(-1);
                if (model.logInUserRole == "Doctor")
                {
                    var YesterDayCheckedParients = _context.Prescriptions.Where(x => x.DoctorId == model.logInUserId && (x.CreatedOn >= yesterday && x.CreatedOn <= endOfYesterday));
                    var YesterdayUnCheckedParient = _context.Appointment.Where(x => x.DoctorId == model.logInUserId && x.CheckUpStatus == false && (x.AppointmentDate >= startOfYesterday && x.AppointmentDate <= endOfYesterday));

                    vM_PreviousDayPatientsRecordCount.PreviousDayPatientChecked = YesterDayCheckedParients.Count();
                    vM_PreviousDayPatientsRecordCount.PreviousDayPatientUnChecked = YesterdayUnCheckedParient.Count();
                }
                else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
                {
                    var YesterDayCheckedParients = _context.Prescriptions.Where(x => x.CreatedOn >= yesterday && x.CreatedOn <= endOfYesterday);
                    var YesterdayUnCheckedParient = _context.Appointment.Where(x => x.CheckUpStatus == false && (x.AppointmentDate >= startOfYesterday && x.AppointmentDate <= endOfYesterday));

                    vM_PreviousDayPatientsRecordCount.PreviousDayPatientChecked = YesterDayCheckedParients.Count();
                    vM_PreviousDayPatientsRecordCount.PreviousDayPatientUnChecked = YesterdayUnCheckedParient.Count();
                }
                _response.Data = vM_PreviousDayPatientsRecordCount;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ErrorWhileGettingData;
                return _response;
            }
        }

        public async Task<IResponse> GetLastTwoWeekPatientRecord(LastTwoWeekPatientCount model)
        {
            VM_LastTwoeWeekPatientCount vM_LastTwoeWeekPatientCount = new VM_LastTwoeWeekPatientCount();
            var today = DateTime.UtcNow.Date;

            // Adjust to Monday of the current week (even if today is Sunday)
            var currentWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                currentWeekStart = today.AddDays(-6); // Shift back to last Monday
            }
            var previousWeekStart = currentWeekStart.AddDays(-7);
            var previousWeekEnd = currentWeekStart.AddDays(-1);
            if (model.logInUserRole == "Doctor")
            {
                var Current7daysparient = _context.Appointment.Where(x => x.DoctorId == model.logInUserId && (x.AppointmentDate >= currentWeekStart && x.AppointmentDate <= DateTime.Now.Date));
                var PreviousWeekparient = _context.Appointment.Where(x => x.DoctorId == model.logInUserId && (x.AppointmentDate >= previousWeekStart && x.AppointmentDate <= previousWeekEnd));

                vM_LastTwoeWeekPatientCount.LastWeekPatientCount = Current7daysparient.Count();
                vM_LastTwoeWeekPatientCount.SecondLastWeekPatientCount = PreviousWeekparient.Count();
            }
            else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
            {
                var Current7daysparient = _context.Appointment.Where(x => x.AppointmentDate >= currentWeekStart && x.AppointmentDate <= DateTime.Now.Date);
                var PreviousWeekparient = _context.Appointment.Where(x => x.AppointmentDate >= previousWeekStart && x.AppointmentDate <= previousWeekEnd);
                vM_LastTwoeWeekPatientCount.LastWeekPatientCount = Current7daysparient.Count();
                vM_LastTwoeWeekPatientCount.SecondLastWeekPatientCount = PreviousWeekparient.Count();
            }
            _response.Data = vM_LastTwoeWeekPatientCount;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;
        }

        public async Task<IResponse> GetAllPatientWithFeeCurrentWeek(CurrentWeekPatientWithFeeCount model)
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                // Adjust to Monday of the current week (even if today is Sunday)
                var currentWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
                if (today.DayOfWeek == DayOfWeek.Sunday)
                {
                    currentWeekStart = today.AddDays(-6); // Shift back to last Monday
                }
                VM_AllPatientWithFeeCount vM_AllPatientWithFee = new VM_AllPatientWithFeeCount();
                if (model.logInUserRole == "Doctor")
                {
                    var totalPatient = await (from ap in _context.Appointment
                                              join p in _context.Patient on ap.PatientId equals p.PatientId
                                              join pr in _context.Prescriptions on ap.PatientId equals pr.PatientId
                                              where pr.DoctorId == model.logInUserId
                                              && (ap.AppointmentDate >= currentWeekStart && ap.AppointmentDate <= today)
                                              select new
                                              {
                                                  ap.PatientId,
                                                  ap.DoctorFee
                                              }).ToListAsync();

                    var allpatientDoctorFee = totalPatient.Sum(x => x.DoctorFee);

                    vM_AllPatientWithFee.PatientCount = totalPatient.Count();
                    vM_AllPatientWithFee.PatientFeeCount = allpatientDoctorFee;
                }
                else if (model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
                {
                    var totalPatient = await (from ap in _context.Appointment
                                              join p in _context.Patient on ap.PatientId equals p.PatientId
                                              join pr in _context.Prescriptions on ap.PatientId equals pr.PatientId
                                              where ap.AppointmentDate >= currentWeekStart && ap.AppointmentDate <= today
                                              select new
                                              {
                                                  ap.PatientId,
                                                  ap.DoctorFee
                                              }).ToListAsync();

                    var allpatientDoctorFee = totalPatient.Sum(x => x.DoctorFee);

                    vM_AllPatientWithFee.PatientCount = totalPatient.Count();
                    vM_AllPatientWithFee.PatientFeeCount = allpatientDoctorFee;
                }
                _response.Data = vM_AllPatientWithFee;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.GetData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.ErrorWhileGettingData;
                return _response;
            }
        }
        List<decimal> GetMonthlyData(IEnumerable<IGrouping<int, Prescription>> groupedData)
        {
            var monthlyData = new decimal[12]; // Initialize an array with 12 months (default values are 0)
            foreach (var group in groupedData)
            {
                // Set the correct month data (Month - 1 because array index starts from 0)
                monthlyData[group.Key - 1] = group.Count();
            }
            return monthlyData.ToList();
        }
    }
}
