using Microsoft.AspNetCore.Identity;
using ShanesCloud.Core;
using ShanesCloud.Data;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Queries;

namespace ShanesCloud.Auth.Authentications.AuthenticateUserWithRefreshToken;

public class AuthenticateUserWithRefreshTokenHandler: ICommandHandler<AuthenticateUserWithRefreshTokenCommand>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithRefreshTokenHandler(IDataContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    #endregion

    #region Private Methods

    private static Result Failure()
    {
        return Result.Failure(Error.NotFound(nameof(User)));
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(AuthenticateUserWithRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                     .WithParams(command.UserId)
                                     .ExecuteAsync(cancellationToken);

        if (user == null)
        {
            return Failure();
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Failure();
        }

        user.LoginOn = DateTimeOffset.UtcNow;

        await _dataContext.SaveChanges(cancellationToken);

        return Result.Success();
    }

    #endregion
}
