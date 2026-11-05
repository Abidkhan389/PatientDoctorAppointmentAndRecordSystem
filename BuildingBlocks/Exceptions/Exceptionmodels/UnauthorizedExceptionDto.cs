namespace BuildingBlocks.Exceptions.Exceptionmodels;
public class UnauthorizedExceptionDto : Exception
{
    public UnauthorizedExceptionDto(string message) : base(message) { }
}