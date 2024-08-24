using MediatR;

namespace ShanesStorage.Core.Exchanges;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
