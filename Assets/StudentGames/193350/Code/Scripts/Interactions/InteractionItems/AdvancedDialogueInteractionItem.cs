using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdvancedDialogueInteractionItem : InteractionItem
{
    public GameEntity performer;

    [TextArea]
    public string message;

    public DialogueAnswer answer1;
    public DialogueAnswer answer2;
}

[System.Serializable]
public class DialogueAnswer
{
    public string text;

    [SerializeReference, SubclassSelector]
    public List<InteractionItem> actions;
}