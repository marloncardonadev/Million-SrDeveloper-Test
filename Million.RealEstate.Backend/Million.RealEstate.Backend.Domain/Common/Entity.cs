namespace Million.RealEstate.Backend.Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }

    protected Entity()
    {
        
    }
}
