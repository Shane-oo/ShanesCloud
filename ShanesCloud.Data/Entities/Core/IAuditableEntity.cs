namespace ShanesCloud.Data.Entities.Core;

public interface IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
}
