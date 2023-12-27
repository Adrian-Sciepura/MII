using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerBehaviourSystem : IBehaviourSystem
{
    private CircleCollider2D _interactionTrigger;
    private GameEntity _player;
    public void Update()
    {

    }

    public void SetContext(GameEntity context)
    {
        GameDataManager.input.Player.Interaction.performed += InteractionButtonClicked;

        _player = context;

        /*_interactionTrigger = context.gameObject.AddComponent<CircleCollider2D>();

        _interactionTrigger.radius = 2.5f;
        _interactionTrigger.offset = Vector3.zero;
        _interactionTrigger.isTrigger = true;*/
    }

    public void OnInteractionAreaEnter(Collider2D other)
    {
        if(other.CompareTag("InteractionTrigger"))
            InteractionManager.AddPossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }

    public void OnInteractionAreaExit(Collider2D other)
    {
        if (other.CompareTag("InteractionTrigger"))
            InteractionManager.RemovePossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }

    public void Dispose()
    {
        GameDataManager.input.Player.Interaction.performed -= InteractionButtonClicked;
        //Object.Destroy(_interactionTrigger);
    }

    private void InteractionButtonClicked(InputAction.CallbackContext context)
    {
        InteractionManager.StartNearestInteraction();
    }
}