public class OnEntityDamageEvent
{
    public GameEntity entity {  get; private set; }
    public int damage { get; private set; }

    public OnEntityDamageEvent(GameEntity entity, int damage)
    {
        this.entity = entity;
        this.damage = damage;
    }
}

public record OnEntityDieEvent(GameEntity Entity);

public class OnEntityPickupItemEvent
{
    public GameEntity entity { get; private set; }
    public Item item { get; private set; }

    public OnEntityPickupItemEvent(GameEntity entity, Item item) 
    {
        this.entity = entity;
        this.item = item;
    }
}

public class OnEntityChangeHeldItemEvent
{
    public GameEntity entity { get; private set; }
    public int previousSlot { get; private set; }

    public OnEntityChangeHeldItemEvent(GameEntity entity, int previousSlot)
    {
        this.entity = entity;
        this.previousSlot = previousSlot;
    }
}