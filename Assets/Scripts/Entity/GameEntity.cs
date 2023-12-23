using UnityEngine;

public enum GameEntityCategory
{
    Player,
    Enemy,
    Boss,
    DroppedItem,
    Block
}

public class GameEntity : MonoBehaviour
{
    public readonly DataContainer<EntityData> entityData = new DataContainer<EntityData>();
    public readonly Inventory inventory = new Inventory();

    private int _heldItemInventorySlot = 0;
    private IBehaviourSystem _behaviourSystem;
    private IMovementSystem _movementSystem;

    public int HeldItemInventorySlot 
    { 
        get => _heldItemInventorySlot;
        set
        {
            if(value >= 0 && value < inventory.size)
                _heldItemInventorySlot = value;
        }
    }

    public IBehaviourSystem BehaviourSystem
    {
        get => _behaviourSystem;
        set
        {
            _behaviourSystem = value;
            _behaviourSystem.SetContext(this);
        }
    }

    public IMovementSystem MovementSystem
    {
        get => _movementSystem;
        set
        {
            if (_movementSystem != null)
                _movementSystem.Dispose();

            _movementSystem = value;
            _movementSystem.SetContext(this);
        }
    }

    private void Update()
    {
        _behaviourSystem?.Update();
        _movementSystem?.Update();
    }

    public void NextItem()
    {
        if (_heldItemInventorySlot >= inventory.size)
            _heldItemInventorySlot = 0;
        else
            _heldItemInventorySlot++;
    }

    public void PreviousItem()
    {
        if (_heldItemInventorySlot <= 0)
            _heldItemInventorySlot = inventory.size - 1;
        else
            _heldItemInventorySlot--;
    }
}
