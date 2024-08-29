using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShanesCloud.Data;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Queries;

namespace ShanesCloud.Users.Users;

public class UserStore: IUserRoleStore<User>, IUserPasswordStore<User>, IUserLockoutStore<User>, IUserEmailStore<User>
{
    #region Fields

    private IDataContext _dataContext;

    private bool _disposed;

    private IdentityErrorDescriber _errorDescriber = new();

    private IRoleStore<Role> _roleStore;

    #endregion

    #region Construction

    public UserStore(IDataContext dataContext, IRoleStore<Role> roleStore)
    {
        _dataContext = dataContext;
        _roleStore = roleStore;
    }

    #endregion

    #region Private Methods

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    #endregion

    #region Public Methods

    public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        ArgumentNullException.ThrowIfNull(role);

        user.AddRole(role.Id);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        await _dataContext.Add(user, cancellationToken);

        await _dataContext.SaveChanges(cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        _dataContext.Remove(user);

        try
        {
            await _dataContext.SaveChanges(cancellationToken);
        }
        catch(DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _disposed = true;
        _roleStore = null;
        _dataContext = null;
        _errorDescriber = null;
    }

    public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(normalizedEmail);

        return await _dataContext.Query<IUserByEmailDbQuery>()
                                 .WithParams(normalizedEmail)
                                 .ExecuteAsync(cancellationToken);
    }

    public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(userId);


        return await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(new UserId(Guid.Parse(userId)))
                                 .ExecuteAsync(cancellationToken);
    }

    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(normalizedUserName);


        return await _dataContext.Query<IUserByUserNameDbQuery>()
                                 .WithParams(normalizedUserName)
                                 .ExecuteAsync(cancellationToken);
    }

    public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.AccessFailedCount);
    }

    public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.LockoutEnabled);
    }

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.LockoutEnd);
    }

    public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        return Task.FromResult(user.NormalizedEmail);
    }

    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.Password);
    }

    // Only One role will be returned
    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(user.Id)
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .ExecuteAsync(cancellationToken);

        var roleName = user?.UserRole.Role?.Name;

        if (string.IsNullOrEmpty(roleName))
        {
            return [];
        }

        return [roleName];
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(user.UserName);
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(roleName);

        return await _dataContext.Query<IUsersByRoleNamesDbQuery>()
                                 .WithParams(roleName)
                                 .ExecuteAsync(cancellationToken);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);


        return Task.FromResult(!string.IsNullOrEmpty(user.Password));
    }

    public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.AccessFailed();

        return Task.FromResult(user.AccessFailedCount);
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                 .WithParams(user.Id)
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .ExecuteAsync(cancellationToken);

        if (user.UserRole == null)
        {
            return false;
        }

        return user.UserRole.Role.Name == roleName;
    }

    public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.UserRole = null;

        await _dataContext.SaveChanges(cancellationToken);
    }

    public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.AccessSuccess();

        return Task.CompletedTask;
    }

    public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(email);

        user.Email = email;

        return Task.CompletedTask;
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.EmailConfirmed = confirmed;

        return Task.CompletedTask;
    }

    public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.LockoutEnabled = enabled;

        return Task.CompletedTask;
    }

    public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        user.LockoutEnd = lockoutEnd;

        return Task.CompletedTask;
    }

    public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(normalizedEmail);

        user.NormalizedEmail = normalizedEmail;

        return Task.CompletedTask;
    }

    public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(normalizedName);

        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(passwordHash);

        user.Password = passwordHash;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(userName);

        user.UserName = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(user);

        _dataContext.Attach(user);
        user.ConcurrencyStamp = Guid.NewGuid();
        _dataContext.Update(user);

        try
        {
            await _dataContext.SaveChanges(cancellationToken);
        }
        catch(DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    #endregion
}
