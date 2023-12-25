using UnityEngine;

[System.Serializable]
public class DialogueInteractionItem : IInteractionItem
{
    [SerializeField]
    public string speakerGUID;

    [SerializeField]
    public string message;


    public void Perform()
    {
        OverlayManager.ShowDialogue(message);
    }

    public void Update()
    {
        
    }

    public bool Finished()
    {
        return !OverlayManager.IsDialogueBoxShown;
    }
}