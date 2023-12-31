public record OnInteractionStartEvent(InteractionTrigger Interaction);

public record OnInteractionFinishEvent();

public record OnInteractionItemStartEvent<T>(T Data) where T : IInteractionItem;

public record OnInteractionItemFinishEvent<T>() where T : IInteractionItem;