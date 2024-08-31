using Microsoft.EntityFrameworkCore;

namespace ShanesCloud.Data.Entities.Queries;

public interface IUserByEmailDbQuery: IDbQuery<User>
{
    IUserByEmailDbQuery Include(params string[] include);

    IUserByEmailDbQuery WithParams(string normalizedEmail);
}

public class UserByEmailDbQuery: IUserByEmailDbQuery
{
    #region Fields

    private readonly Context _context;
    private string[] _include;

    private string _normalizedEmail;

    #endregion

    #region Construction

    public UserByEmailDbQuery(Context context)
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

        return await query.FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(_normalizedEmail), cancellationToken);
    }

    public IUserByEmailDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByEmailDbQuery WithParams(string normalizedEmail)
    {
        _normalizedEmail = normalizedEmail;
        return this;
    }

    #endregion
}
