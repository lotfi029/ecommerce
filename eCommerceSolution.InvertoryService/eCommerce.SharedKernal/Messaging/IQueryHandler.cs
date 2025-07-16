using eCommerce.SharedKernal.Abstractions;

namespace eCommerce.SharedKernal.Messaging;
public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct = default);
}