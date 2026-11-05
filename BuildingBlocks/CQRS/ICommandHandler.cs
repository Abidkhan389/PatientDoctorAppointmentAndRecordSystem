using BuildingBlocks.Common.Response.Response;
using MediatR;

namespace BuildingBlocks.CQRS;
public interface ICommandHandler<in TCommand>
    : ICommandHandler<TCommand, IResponse>
    where TCommand : ICommand<IResponse>
{
}

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : IResponse
{
}
