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
    public class DashboardRepository: IDashboardRepository
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
            if (model.logInUserRole == "Doctor")
            {
                var CurrentMonthparient = _context.Prescriptions.Where(x => x.DoctorId == model.logInUserId && (x.CreatedOn >= DateTime.Now.Date.AddDays(-30) && x.CreatedOn <= DateTime.Now.Date.AddDays(1)));
                var CurrentWeekparient = CurrentMonthparient.Where(x => x.DoctorId == model.logInUserId && (x.CreatedOn >= DateTime.Now.Date.AddDays(-7) && x.CreatedOn <= DateTime.Now.Date.AddDays(1)));

                vM_CurrentWeekAndMonth.CurrentMonthPatientCount = CurrentMonthparient.Count();
                vM_CurrentWeekAndMonth.CurrentWeekPatientCount = CurrentWeekparient.Count();
            }
            else if(model.logInUserRole.Contains("Admin") || model.logInUserRole.Contains("SuperAdmin"))
            {
                var CurrentMonthparient = _context.Prescriptions.Where(x => x.CreatedOn >= DateTime.Now.Date.AddDays(-30) && x.CreatedOn <= DateTime.Now.Date.AddDays(1));
                var CurrentWeekparient = CurrentMonthparient.Where(x => x.CreatedOn >= DateTime.Now.Date.AddDays(-7) && x.CreatedOn <= DateTime.Now.Date.AddDays(1));
                vM_CurrentWeekAndMonth.CurrentMonthPatientCount = CurrentMonthparient.Count();
                vM_CurrentWeekAndMonth.CurrentWeekPatientCount = CurrentWeekparient.Count();
            }
            _response.Data = vM_CurrentWeekAndMonth;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;
        }
    }
}
