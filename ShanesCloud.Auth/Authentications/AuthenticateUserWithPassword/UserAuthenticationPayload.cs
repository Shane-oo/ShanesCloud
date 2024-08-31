using ShanesCloud.Data.Entities;

namespace ShanesCloud.Auth.Authentications;

public class UserAuthenticationPayload
{
    #region Properties

    public required Roles Role { get; set; }

    public required UserId UserId { get; set; }

    public required string Username { get; set; }

    #endregion
}
