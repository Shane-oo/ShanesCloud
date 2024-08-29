using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using ShanesCloud.Core;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Users.Users;

public class CreateUserHandler: ICommandHandler<CreateUserCommand>
{
    #region Fields

    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public CreateUserHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    #endregion

    #region Private Methods

    private async Task<Result> AddUserToRole(User user, Roles role)
    {
        var roleName = role.ToString();

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var newRole = new Role
                          {
                              Name = roleName
                          };
            var createNewRoleResult = await _roleManager.CreateAsync(newRole);
            if (!createNewRoleResult.Succeeded)
            {
                return GetIdentityResultError(createNewRoleResult);
            }
        }

        var addRoleToUserResult = await _userManager.AddToRoleAsync(user, roleName);
        return !addRoleToUserResult.Succeeded
                   ? GetIdentityResultError(addRoleToUserResult)
                   : Result.Success();
    }

    private async Task<Result> CreateUserWithoutPassword(User user, Roles role)
    {
        var userCreateResult = await _userManager.CreateAsync(user);
        if (!userCreateResult.Succeeded)
        {
            return GetIdentityResultError(userCreateResult);
        }

        return await AddUserToRole(user, role);
    }

    private async Task<Result> CreateUserWithPassword(User user, string password, Roles role)
    {
        var userCreateResult = await _userManager.CreateAsync(user, password);
        if (!userCreateResult.Succeeded)
        {
            return GetIdentityResultError(userCreateResult);
        }

        return await AddUserToRole(user, role);
    }

    private static Result GetIdentityResultError(IdentityResult identityResult)
    {
        var identityError = identityResult.Errors.FirstOrDefault();
        return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle([NotNull] CreateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = new User
                   {
                       Email = command.Email,
                       UserName = command.UserName
                   };

        if (command.Role is Roles.Admin or Roles.User)
        {
            return await CreateUserWithPassword(user, command.Password, command.Role);
        }

        return await CreateUserWithoutPassword(user, command.Role);
    }

    #endregion
}
