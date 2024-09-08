using Microsoft.AspNetCore.Identity;
using ShanesCloud.Core;
using ShanesCloud.Data;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Queries;

namespace ShanesCloud.Auth.Authentications;

public class AuthenticateUserWithPasswordHandler: ICommandHandler<AuthenticateUserWithPasswordCommand, UserAuthenticationPayload>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithPasswordHandler(IDataContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    #endregion

    #region Private Methods

    private static Result<UserAuthenticationPayload> Reject()
    {
        return Result.Failure<UserAuthenticationPayload>(Error.NotFound(nameof(User)));
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserAuthenticationPayload>> Handle(AuthenticateUserWithPasswordCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await _dataContext.Query<IUserByEmailDbQuery>()
                                     .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                     .WithParams(command.Email)
                                     .ExecuteAsync(cancellationToken);

        if (user == null)
        {
            return Reject();
        }

        if (user.UserRole.Role.RoleEnum == Roles.Guest)
        {
            return Reject();
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Reject();
        }

        if (!await _userManager.CheckPasswordAsync(user, command.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Reject();
        }

        user.LoginOn = DateTime.UtcNow;

        // saves changes
        await _userManager.ResetAccessFailedCountAsync(user);

        return new UserAuthenticationPayload
               {
                   Role = user.UserRole.Role.RoleEnum,
                   UserId = user.Id,
                   Username = user.UserName
               };
    }

    #endregion
}
