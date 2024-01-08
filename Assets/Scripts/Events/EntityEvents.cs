public record OnEntityDamageEvent(GameEntity Entity, int Damage);

public record OnEntityHealEvent(GameEntity Entity);

public record OnEntityDieEvent(GameEntity Entity);

public record OnEntityPickupItemEvent(GameEntity Entity, ItemType Item);

public record OnEntityChangeHeldItemEvent(GameEntity Entity, int PreviousSlot);