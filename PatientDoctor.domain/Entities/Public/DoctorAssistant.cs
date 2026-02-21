
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.domain.Entities.Public;
[Table("DoctorAssistant", Schema = "Admin")]
public class DoctorAssistant:LogFields
{
    public Guid Id { get; set; }
    //FK to Doctor
    public string? DoctorId { get; set; }    
    public ApplicationUser? Doctor { get; set; }
    //FK to Assistant
    public string? AssistantId { get; set; }
    public ApplicationUser? Assistant { get; set; }
}

