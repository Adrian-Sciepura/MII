public record OnEntityDamageEvent(GameEntity entity, int Damage);

public record OnEntityDieEvent(GameEntity Entity);

public record OnEntityPickupItemEvent(GameEntity Entity, Item Item);

public record OnEntityChangeHeldItemEvent(GameEntity Entity, int PreviousSlot);