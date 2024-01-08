using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviourSystem : BehaviourSystem
{
    private GameEntity _entity;
    private LivingEntityData _healthData;
    private Collider2D _mainCollider;

    private void InteractionButtonClicked(InputAction.CallbackContext context)
    {
        InteractionManager.StartNearestInteraction();
    }

    private void InventorySlotButtonClicked(InputAction.CallbackContext context)
    {
        int slotIndex = int.Parse(context.control.name) - 1;

        if (slotIndex == _entity.HeldItemInventorySlot)
            return;

        int oldIndex = _entity.HeldItemInventorySlot;
        _entity.HeldItemInventorySlot = slotIndex;
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        _entity.HeldItem?.Use();
    }

    protected override void Awake()
    {
        GameDataManager.input.Player.Interaction.performed += InteractionButtonClicked;
        GameDataManager.input.Player.Inventory.performed += InventorySlotButtonClicked;
        GameDataManager.input.Player.UseItem.performed += UseItem;

        _mainCollider = GetComponent<Collider2D>();
        _entity = GetComponent<GameEntity>();
        _healthData = GetComponent<EntityDataContainer>().GetData<LivingEntityData>();
    }

    protected override void Update()
    {
        
    }

    protected override void OnDestroy()
    {
        GameDataManager.input.Player.Interaction.performed -= InteractionButtonClicked;
        GameDataManager.input.Player.Inventory.performed -= InventorySlotButtonClicked;
        GameDataManager.input.Player.UseItem.performed -= UseItem;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("InteractionTrigger"))
            InteractionManager.AddPossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InteractionTrigger"))
            InteractionManager.RemovePossibleInteraction(other.gameObject.GetComponent<InteractionTrigger>());
    }
}