using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc
{
	public class VM_DoctorCheckUpFeeDetails : ListingLogFields
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string DocotrId { get; set; }
        public int DocterFee { get; set; }
        public int Status { get; set; }
    }
}

