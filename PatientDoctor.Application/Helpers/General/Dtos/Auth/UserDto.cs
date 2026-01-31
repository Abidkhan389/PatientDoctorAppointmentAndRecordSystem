namespace PatientDoctor.Application.Helpers.General.Dtos.Auth;
public class UserDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public IList<string>? Roles { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

