using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
    public class AddPatientDescriptionCommand : IRequest<IResponse>
    {
        public Guid? Id { get; set; }
        public Guid PatientId { get; set; }
        public string DoctorId { get; set; }
        public Guid? UserId { get; set; }
        public string? ComplaintOf { get; set; }
        public string? Diagnosis { get; set; }
        public string? Plan { get; set; }
        public string? LeftVision { get; set; }
        public string? RightVision { get; set; }
        public string? LeftMG { get; set; }
        public string? RightMG { get; set; }
        public string? LeftEOM { get; set; }
        public string? RightEom { get; set; }
        public string? LeftOrtho { get; set; }
        public string? RightOrtho { get; set; }
        public string? LeftTension { get; set; }
        public string? RightTension { get; set; }
        public string? LeftAntSegment { get; set; }
        public string? RightAntSegment { get; set; }
        public string? LeftDisc { get; set; }
        public string? RightDisc { get; set; }
        public string? LeftMacula { get; set; }
        public string? RightMacula { get; set; }
        public string? LeftPeriphery { get; set; }
        public string? RightPeriphery { get; set; }
        public List<MedicineOptions>? medicine { get; set; } = new List<MedicineOptions>();
    }
    public class MedicineOptions
    {
        public Guid? Id { get; set; }
        public Guid MedicineId { get; set; }
        public int DurationInDays { get; set; }
        public bool? Morning { get; set; }
        public bool? Afternoon { get; set; }
        public bool? Evening { get; set; }
        public bool? Night { get; set; }
        public bool? RepeatEveryHours { get; set; }
        public bool? RepeatEveryTwoHours { get; set; }
    }

