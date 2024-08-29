using Microsoft.EntityFrameworkCore;

namespace ShanesCloud.Data.Entities.Queries;

public interface IUserByUserIdDbQuery: IDbQuery<User>
{
    IUserByUserIdDbQuery WithParams(UserId userId);

    IUserByUserIdDbQuery Include(params string[] include);
}

public class UserByUserIdDbQuery: IUserByUserIdDbQuery
{
    #region Fields

    private readonly Context _context;
    private string[] _include;
    private UserId _userId;

    #endregion

    #region Construction

    public UserByUserIdDbQuery(Context context)
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

        return await query.FirstOrDefaultAsync(u => u.Id == _userId, cancellationToken);
    }

    public IUserByUserIdDbQuery WithParams(UserId userId)
    {
        _userId = userId;
        return this;
    }

    public IUserByUserIdDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    #endregion
}
