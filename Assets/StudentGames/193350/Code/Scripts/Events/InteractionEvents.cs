public record OnInteractionStartEvent(InteractionTrigger Interaction);

public record OnInteractionFinishEvent();

public record OnInteractionItemStartEvent<T>(T Data) where T : InteractionItem;

public record OnInteractionItemFinishEvent<T>() where T : InteractionItem;