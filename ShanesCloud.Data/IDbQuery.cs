namespace ShanesCloud.Data;

public interface IDbQuery;

public interface IDbQuery<TResult>: IDbQuery
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}
