using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InteractionTrigger : MonoBehaviour
{
    [SerializeField]
    public InteractionParams interactionParams;

    [SerializeReference, SubclassSelector]
    public List<IInteractionCondition> conditions;

    [SerializeReference, SubclassSelector]
    public List<InteractionItem> content;

    public bool CheckConditions() => !conditions.Any(condition => !condition.CheckCondition());
}
