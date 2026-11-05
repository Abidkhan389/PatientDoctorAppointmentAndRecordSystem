namespace BuildingBlocks.Exceptions.Exceptionmodels;
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}