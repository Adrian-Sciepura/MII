using UnityEngine;

[System.Serializable]
public class DialogueInteractionItem : IInteractionItem
{
    [SerializeField]
    private string performerGUID;

    [SerializeField]
    private string _message;

    public void Perform()
    {
        GameEntity senderEntity;

        if (!LevelManager.spawnedEntities.TryGetValue(performerGUID, out senderEntity))
            return;

        OverlayManager.ShowDialogue(senderEntity.gameObject.GetComponent<SpriteRenderer>().sprite, GameDataManager.entityRegistry[senderEntity.entityType].displayName, _message);
    }

    public void Update()
    {
        
    }

    public bool Finished()
    {
        return !OverlayManager.IsDialogueBoxShown;
    }
}
