namespace ShanesCloud.Data.Entities.Core;

public interface IEntity;

public abstract class Entity<TEntityId>: IEntity
{
    #region Properties

    public TEntityId Id { get; set; }

    #endregion

    #region Construction

    protected Entity()
    {
    }

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return $"{Id}";
    }

    #endregion
}
