
namespace PatientDoctor.Application.Helpers.General.Exceptions;
internal class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}
