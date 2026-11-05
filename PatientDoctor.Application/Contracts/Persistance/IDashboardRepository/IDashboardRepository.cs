namespace PatientDoctor.Application.Contracts.Persistance.Dashboard
{
    public interface IDashboardRepository
    {
        Task<IResponse> GetOverViewForAdminDashboard();
        Task<IResponse> GetPatientCurrentWeekAndMonth(WelComeCurrentWeekAndMonth model);
        Task<IResponse> GetAllPatientWithFee(AllPatientWithFeeCount model);
        Task<IResponse> GetPatientCountYearlyWise(PatientCountYearWise model);
        Task<IResponse> GetPreviousDayPatientsRecord(PreviousDayPatientsRecord model);
        Task<IResponse> GetLastTwoWeekPatientRecord(LastTwoWeekPatientCount model);
        Task<IResponse> GetAllPatientWithFeeCurrentWeek(CurrentWeekPatientWithFeeCount model);
        


    }
}
