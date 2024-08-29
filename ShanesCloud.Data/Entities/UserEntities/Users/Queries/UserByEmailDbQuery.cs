using Microsoft.EntityFrameworkCore;

namespace ShanesCloud.Data.Entities.Queries;

public interface IUserByEmailDbQuery: IDbQuery<User>
{
    IUserByEmailDbQuery WithParams(string normalizedEmail);
}

public class UserByEmailDbQuery: IUserByEmailDbQuery
{
    #region Fields

    private readonly Context _context;

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
        return await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(_normalizedEmail), cancellationToken);
    }

    public IUserByEmailDbQuery WithParams(string normalizedEmail)
    {
        _normalizedEmail = normalizedEmail;
        return this;
    }

    #endregion
}
