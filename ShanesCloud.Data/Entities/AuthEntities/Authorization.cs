using OpenIddict.EntityFrameworkCore.Models;
using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data.Entities;

public class Authorization: OpenIddictEntityFrameworkCoreAuthorization<Guid, Application, Token>, IEntity, IAuditableEntity
{
    #region Properties

    public Guid ApplicationId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    #endregion
}
