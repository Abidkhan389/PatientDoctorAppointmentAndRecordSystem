
using BuildingBlocks.Common.Response.Response;
using MediatR;

namespace BuildingBlocks.CQRS;
// Query jo IResponse return kare
public interface IQuery : IQuery<IResponse> { }

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : IResponse
{
}
