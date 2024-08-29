﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShanesCloud.Data;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Queries;

namespace ShanesCloud.Users.Users;

public class RoleStore: IRoleStore<Role>
{
    #region Fields

    private IDataContext _dataContext;

    private bool _disposed;

    private IdentityErrorDescriber _errorDescriber = new();

    #endregion

    #region Construction

    public RoleStore(IDataContext dataContext)
    {
        _dataContext = dataContext;
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

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(role);

        await _dataContext.Add(role, cancellationToken);
        await _dataContext.SaveChanges(cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(role);

        _dataContext.Remove(role);

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
        _dataContext = null;
        _errorDescriber = null;
    }

    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(roleId);

        return await _dataContext.Query<IRoleByRoleIdDbQuery>()
                                 .WithParams(new RoleId(Guid.Parse(roleId)))
                                 .ExecuteAsync(cancellationToken);
    }

    public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentException.ThrowIfNullOrEmpty(normalizedRoleName);

        return await _dataContext.Query<IRoleByNameDbQuery>()
                                 .WithParams(normalizedRoleName)
                                 .ExecuteAsync(cancellationToken);
    }

    public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        ArgumentNullException.ThrowIfNull(role);

        _dataContext.Attach(role);
        role.ConcurrencyStamp = Guid.NewGuid();
        _dataContext.Update(role);

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
