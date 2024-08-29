using Microsoft.EntityFrameworkCore;

namespace ShanesCloud.Data.Entities.Queries;

public interface IUserByUserNameDbQuery: IDbQuery<User>
{
    IUserByUserNameDbQuery Include(params string[] include);

    IUserByUserNameDbQuery WithParams(string normalizedUserName);
}

public class UserByUserUserNameDbQuery: IUserByUserNameDbQuery
{
    #region Fields

    private readonly Context _context;

    private string[] _include;
    private string _normalizedUserName;

    #endregion

    #region Construction

    public UserByUserUserNameDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        return await query.FirstOrDefaultAsync(u => u.NormalizedUserName.Equals(_normalizedUserName),
                                               cancellationToken);
    }

    public IUserByUserNameDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByUserNameDbQuery WithParams(string normalizedUserName)
    {
        _normalizedUserName = normalizedUserName;
        return this;
    }

    #endregion
}
