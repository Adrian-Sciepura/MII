using UnityEngine;

[System.Serializable]
public class DialogueInteractionItem : InteractionItem
{
    public GameEntity performer;
    
    [TextArea]
    public string message;
}
