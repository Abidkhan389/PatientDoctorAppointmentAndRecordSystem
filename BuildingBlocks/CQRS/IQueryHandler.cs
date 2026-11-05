
using BuildingBlocks.Common.Response.Response;
using MediatR;

namespace BuildingBlocks.CQRS;
// QueryHandler jo IResponse return kare
public interface IQueryHandler<in TQuery>
    : IQueryHandler<TQuery, IResponse>
    where TQuery : IQuery<IResponse>
{
}

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : IResponse
{
}
