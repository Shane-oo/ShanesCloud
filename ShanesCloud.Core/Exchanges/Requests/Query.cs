using MediatR;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Core.Exchanges;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>;

public class Query<TResponse>: IQuery<TResponse>
{
    #region Properties

    public UserId UserId { get; }

    #endregion

    #region Construction

    public Query(UserId userId)
    {
        UserId = userId;
    }

    public Query()
    {
    }

    #endregion
}
