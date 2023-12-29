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
        GameDataManager.input.Player.Inventory.performed += InventorySlotButtonClicked;
        GameDataManager.input.Player.UseItem.performed += UseItem;

        _player = context;

        /*_interactionTrigger = context.gameObject.AddComponent<CircleCollider2D>();

        _interactionTrigger.radius = 2.5f;
        _interactionTrigger.offset = Vector3.zero;
        _interactionTrigger.isTrigger = true;*/
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if(other.CompareTag("InteractionTrigger"))
            InteractionManager.AddPossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }

    public void OnTriggerLeave(Collider2D other)
    {
        if (other.CompareTag("InteractionTrigger"))
            InteractionManager.RemovePossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }

    public void Dispose()
    {
        GameDataManager.input.Player.Interaction.performed -= InteractionButtonClicked;
        GameDataManager.input.Player.Inventory.performed -= InventorySlotButtonClicked;
        GameDataManager.input.Player.UseItem.performed -= UseItem;
        //Object.Destroy(_interactionTrigger);
    }

    private void InteractionButtonClicked(InputAction.CallbackContext context)
    {
        InteractionManager.StartNearestInteraction();
    }

    private void InventorySlotButtonClicked(InputAction.CallbackContext context)
    {
        int slotIndex = int.Parse(context.control.name) - 1;

        if (slotIndex == _player.HeldItemInventorySlot)
            return;

        int oldIndex = _player.HeldItemInventorySlot;
        _player.HeldItemInventorySlot = slotIndex;

        EventManager.Instance.Publish(new OnEntityChangeHeldItemEvent(_player, oldIndex));
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        _player.GetComponent<Animator>().SetTrigger("swing");
    }
}