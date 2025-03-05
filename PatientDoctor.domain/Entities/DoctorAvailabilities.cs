
using Newtonsoft.Json;
using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using PatientDoctor.domain.Entities.Public;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PatientDoctor.domain.Entities
{
    [Table("DoctorAvailability", Schema = "Admin")]
    public class DoctorAvailabilities : LogFields
    {
        [Key]
        public Guid AvailabilityId { get; set; }

        public string DoctorId { get; set; }
        public int DayId { get; set; }
        public int Status { get; set; }
        public string TimeSlotsJson { get; set; } // Store JSON data

        public int AppointmentDurationMinutes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ApplicationUser Doctor { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        // Helper property for JSON conversion
        [NotMapped]
        public List<DoctorTimeSlot> TimeSlots
        {
            get => string.IsNullOrEmpty(TimeSlotsJson)
                   ? new List<DoctorTimeSlot>()
                   : JsonConvert.DeserializeObject<List<DoctorTimeSlot>>(TimeSlotsJson);
            set => TimeSlotsJson = JsonConvert.SerializeObject(value);
        }
        public DoctorAvailabilities()
        {
            
        }
    }
}


