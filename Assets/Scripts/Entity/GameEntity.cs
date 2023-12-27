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
    public GameEntityType entityType;

    private int _heldItemInventorySlot = 0;
    private IBehaviourSystem _behaviourSystem;
    private IMovementSystem _movementSystem;

    public int HeldItemInventorySlot
    {
        get => _heldItemInventorySlot;
        set
        {
            if (value >= 0 && value < inventory.size)
                _heldItemInventorySlot = value;
        }
    }

    public IBehaviourSystem BehaviourSystem
    {
        get => _behaviourSystem;
        set
        {
            if (_behaviourSystem != null)
                _behaviourSystem.Dispose();

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
        _behaviourSystem.Update();
        _movementSystem.Update();
    }

    private void OnDestroy()
    {
        _behaviourSystem.Dispose();
        _movementSystem.Dispose();
    }

    private void OnTriggerEnter2D(Collider2D collision) => _behaviourSystem.OnInteractionAreaEnter(collision);

    private void OnTriggerExit2D(Collider2D collision) => _behaviourSystem.OnInteractionAreaExit(collision);

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
