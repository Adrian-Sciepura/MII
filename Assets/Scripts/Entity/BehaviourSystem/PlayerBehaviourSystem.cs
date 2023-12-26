using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerBehaviourSystem : IBehaviourSystem
{
    public void Update()
    {

    }

    public void SetContext(GameEntity context)
    {
        GameDataManager.input.Player.Interaction.performed += InteractionButtonClicked;
    }

    public void OnInteractionAreaEnter(GameEntity other)
    {

        InteractionManager.AddPossibleInteraction(other);
    }

    public void OnInteractionAreaExit(GameEntity other)
    {
        InteractionManager.RemovePossibleInteraction(other);
    }

    public void Dispose()
    {
        GameDataManager.input.Player.Interaction.performed -= InteractionButtonClicked;
    }

    private void InteractionButtonClicked(InputAction.CallbackContext context)
    {
        InteractionManager.StartNearestInteraction();
    }
}