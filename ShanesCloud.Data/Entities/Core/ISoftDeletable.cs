namespace ShanesCloud.Data.Entities.Core;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
