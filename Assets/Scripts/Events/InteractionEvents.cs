public class OnInteractionStartEvent
{
    public InteractionTrigger interaction { get; private set; }

    public OnInteractionStartEvent(InteractionTrigger interaction) => this.interaction = interaction;
}

public class OnInteractionFinishEvent
{

}

public class OnInteractionItemStartEvent<T> where T : IInteractionItem
{
    public T data { get; private set; }

    public OnInteractionItemStartEvent(T data) => this.data = data;
}

public class OnInteractionItemFinishEvent<T> where T : IInteractionItem
{
    /*public T data { get; private set; }

    public OnInteractionItemFinishEvent(T data) => this.data = data;*/
}