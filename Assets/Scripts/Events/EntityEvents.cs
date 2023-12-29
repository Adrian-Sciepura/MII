public class OnEntityDamageEvent
{
    public GameEntity entity;
    public int damage;
}

public class OnEntityPickupItemEvent
{
    public GameEntity entity;
    public Item item;
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