namespace PatientDoctor.Application.Features.Administrator.Quries;
public class VM_GetUserProfileByEmailAndId
{
    public string UserId { get; set; }
    public string EmailorPhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string PhoneNumber { get; set; }

}

