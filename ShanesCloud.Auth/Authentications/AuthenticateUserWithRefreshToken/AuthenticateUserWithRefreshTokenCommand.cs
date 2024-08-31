using ShanesCloud.Core;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Auth.Authentications.AuthenticateUserWithRefreshToken;

public class AuthenticateUserWithRefreshTokenCommand: Command
{
    #region Construction

    public AuthenticateUserWithRefreshTokenCommand(UserId userId): base(userId)
    {
    }

    #endregion
}
