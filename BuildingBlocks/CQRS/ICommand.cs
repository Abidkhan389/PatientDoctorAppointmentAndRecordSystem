using BuildingBlocks.Common.Response.Response;
using MediatR;

namespace BuildingBlocks.CQRS;

public interface ICommand : ICommand<IResponse> { }

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : IResponse
{
}
