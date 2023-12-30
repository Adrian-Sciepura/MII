using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerBehaviourSystem : IBehaviourSystem
{
    private GameEntity _context;
    private LivingEntityData _healthData;

    public GameEntity context
    {
        get => _context;
        set
        {
            if (_context != null)
                return;

            _context = value;

            GameDataManager.input.Player.Interaction.performed += InteractionButtonClicked;
            GameDataManager.input.Player.Inventory.performed += InventorySlotButtonClicked;
            GameDataManager.input.Player.UseItem.performed += UseItem;

            _healthData = context.entityData.GetData<LivingEntityData>();
        }
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
        if(context == null)
            return;

        GameDataManager.input.Player.Interaction.performed -= InteractionButtonClicked;
        GameDataManager.input.Player.Inventory.performed -= InventorySlotButtonClicked;
        GameDataManager.input.Player.UseItem.performed -= UseItem;
    }

    private void InteractionButtonClicked(InputAction.CallbackContext context)
    {
        InteractionManager.StartNearestInteraction();
    }

    private void InventorySlotButtonClicked(InputAction.CallbackContext context)
    {
        int slotIndex = int.Parse(context.control.name) - 1;

        if (slotIndex == _context.HeldItemInventorySlot)
            return;

        int oldIndex = _context.HeldItemInventorySlot;
        _context.HeldItemInventorySlot = slotIndex;

        EventManager.Instance.Publish(new OnEntityChangeHeldItemEvent(_context, oldIndex));
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        _context.inventory.items[_context.HeldItemInventorySlot]?.Use();
    }
}