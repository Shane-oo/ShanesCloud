using OpenIddict.EntityFrameworkCore.Models;
using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data.Entities;

public class Token: OpenIddictEntityFrameworkCoreToken<Guid, Application, Authorization>, IEntity, IAuditableEntity
{
    #region Properties

    public Guid ApplicationId { get; set; }

    public Guid AuthorizationId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    #endregion
}
