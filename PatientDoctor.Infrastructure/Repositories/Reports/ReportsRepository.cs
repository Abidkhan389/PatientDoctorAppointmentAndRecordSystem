
namespace PatientDoctor.Infrastructure.Repositories.Reports;
public class ReportsRepository(DocterPatiendDbContext _context, IResponse _response,
                                        UserManager<ApplicationUser> _userManager) : IReports
{
    public async Task<IResponse> GetCheckedPatientHistoryByDoctor(GetCheckedPatientHistoryByDoctorQuery model)
    {
        // Default date range: last 1 year
        if (model.FromDate == DateTime.MinValue)
            model.FromDate = DateTime.Today.AddYears(-1);
        if (model.ToDate == DateTime.MinValue)
            model.ToDate = DateTime.Today;
        // Step 1: Flat data
        var flatData = await(
                from ap in _context.Appointment
                join per in _context.Prescriptions on ap.PatientId equals per.PatientId
                join patient in _context.Patient on ap.PatientId equals patient.PatientId
                join patientDetails in _context.PatientDetails on patient.PatientId equals patientDetails.PatientId
                join doctor in _userManager.Users on ap.DoctorId equals doctor.Id
                join doctorDetails in _context.Userdetail on doctor.Id equals doctorDetails.UserId
                where
                    ap.CheckUpStatus == true &&
                    ap.AppointmentDate >=model.FromDate &&
                    ap.AppointmentDate <=model.ToDate
                select new {
                    // Doctor
                    DoctorId = doctor.Id,
                    DoctorFirstName = doctorDetails.FirstName,
                    DoctorLastName = doctorDetails.LastName,
                    DoctorCnic = doctorDetails.Cnic,
                    DoctorMobile = doctor.PhoneNumber,
                    DoctoerFee= doctorDetails.Fee,
                    // Patient
                    PatientId = patient.PatientId,
                    PatientFirstName = patient.FirstName,
                    PatientLastName = patient.LastName,
                    PatientCnic = patient.Cnic,
                    PatientCity = patientDetails.City,
                    PatientMobile = patientDetails.PhoneNumber,
                    TimeSlot = ap.TimeSlot,
                    DocterDiscountFee = ap.DoctorFee,
                    AppointmentDate = ap.AppointmentDate,
                    PrescriptionId = per.PrescriptionId
                }

            ).ToListAsync();
        // Step 2: Group into required JSON shape
        var grouped = flatData
            .GroupBy(x =>new
            {
                x.DoctorId,
                x.DoctorFirstName,
                x.DoctorLastName,
                x.DoctorCnic,
                x.DoctorMobile
            })
            .Select(g => new
            {
                doctor = new
                {
                    DoctorId= g.Key.DoctorId,
                    DoctorFirstName = g.Key.DoctorFirstName,
                    DoctorLastName = g.Key.DoctorLastName,
                    DoctorCnic= g.Key.DoctorCnic,
                    DoctorMobile = g.Key.DoctorMobile,
                },
                TotalPatients = g.Count(),
                Patients = g.Select(p => new
                {
                    PatientId = p.PatientId,
                    PatientFirstName = p.PatientFirstName,
                    PatientLastName = p.PatientLastName,
                    PatientCnic = p.PatientCnic,
                    PatientCity = p.PatientCity,
                    PatientMobile = p.PatientMobile,
                    TimeSlot = p.TimeSlot,
                    DocterDiscountFee = p.DocterDiscountFee,
                    AppointmentDate = p.AppointmentDate,
                    PrescriptionId = p.PrescriptionId
                }).ToList()
            }).ToList();
        _response.Success = Constants.ResponseSuccess;
        _response.Message = Constants.GetData;
        _response.Data = grouped;
        return _response;
    }
}

