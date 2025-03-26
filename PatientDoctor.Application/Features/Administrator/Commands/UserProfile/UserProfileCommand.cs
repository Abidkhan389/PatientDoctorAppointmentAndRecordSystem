using MediatR;
using Microsoft.AspNetCore.Http;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Administrator.Commands.UserProfile;

public class UserProfileCommand : IRequest<IResponse>
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneNumber { get; set; }
    public string EmailorPhoneNumber { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
    public bool PasswordChange { get; set; }
    public Guid? LoedInUserId { get; set; }
    public IFormFile? File { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }

}

