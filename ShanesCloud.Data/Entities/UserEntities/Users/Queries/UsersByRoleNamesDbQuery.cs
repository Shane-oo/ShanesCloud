using Microsoft.EntityFrameworkCore;

namespace ShanesCloud.Data.Entities.Queries;

public interface IUsersByRoleNamesDbQuery: IDbQuery<List<User>>
{
    IUsersByRoleNamesDbQuery WithNoTracking();

    IUsersByRoleNamesDbQuery WithParams(string roleName);
}

public class UsersByRoleNamesDbQuery: IUsersByRoleNamesDbQuery
{
    #region Fields

    private readonly Context _context;

    private bool _noTracking;
    private string _roleName;

    #endregion

    #region Construction

    public UsersByRoleNamesDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<List<User>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.UserRoles.AsQueryable();

        if (_noTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.Where(ur => ur.Role.Name.Equals(_roleName))
                          .Select(u => u.User)
                          .ToListAsync(cancellationToken);
    }

    public IUsersByRoleNamesDbQuery WithNoTracking()
    {
        _noTracking = true;
        return this;
    }

    public IUsersByRoleNamesDbQuery WithParams(string roleName)
    {
        _roleName = roleName;
        return this;
    }

    #endregion
}
