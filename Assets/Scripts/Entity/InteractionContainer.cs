using System.Collections.Generic;
using System.Linq;

public class InteractionContainer
{
    private List<InteractionSystem> _interactions = new List<InteractionSystem>();

    public InteractionSystem GetInteraction() => _interactions.FirstOrDefault(x => x.CheckConditions());
    
    public void AddInteractions(List<InteractionSystem> interactions)
    {
        foreach(var interaction in interactions)
            _interactions.Add(interaction);

        _interactions.Sort((x1, x2) => x1.interactionParams.Priority.CompareTo(x2.interactionParams.Priority));
    }
}