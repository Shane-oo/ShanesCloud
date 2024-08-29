namespace ShanesCloud.Data.Entities.Core;

public interface ISoftDeletable
{
    public DateTimeOffset? DeletedOn { get; set; }

    public bool IsDeleted { get; set; }
}
