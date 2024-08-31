using OpenIddict.EntityFrameworkCore.Models;
using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data.Entities;

public class Scope: OpenIddictEntityFrameworkCoreScope<Guid>, IEntity, IAuditableEntity
{
    #region Properties

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    #endregion
}
