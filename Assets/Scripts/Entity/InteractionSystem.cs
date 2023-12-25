using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InteractionSystem
{
    [SerializeField]
    public InteractionParams interactionParams = new InteractionParams();

    [SerializeReference, SubclassSelector]
    public List<IInteractionCondition> conditions = new List<IInteractionCondition>();

    [SerializeReference, SubclassSelector]
    public List<IInteractionItem> content = new List<IInteractionItem>();

    public bool CheckConditions() => !conditions.Any(condition => !condition.CheckCondition());
}