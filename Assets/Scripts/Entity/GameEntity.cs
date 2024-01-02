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
    public DataContainer<EntityData> entityData { get; private set; }
    public Inventory inventory { get; private set; }

    private int _heldItemInventorySlot = 0;
    private IBehaviourSystem _behaviourSystem;
    private IMovementSystem _movementSystem;
    private GameEntityType _entityType;
    private string _guid;

    public int HeldItemInventorySlot
    {
        get => _heldItemInventorySlot;
        set
        {
            if (value >= 0 && value < inventory.size)
            {
                inventory.items[_heldItemInventorySlot]?.gameObject.SetActive(false);
                _heldItemInventorySlot = value;
                inventory.items[_heldItemInventorySlot]?.gameObject?.SetActive(true);
            }
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
            _behaviourSystem.context = this;
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
            _movementSystem.context = this;
        }
    }

    public GameEntityType EntityType
    {
        get => _entityType;
        set
        {
            if(_entityType == default)
                _entityType = value;
        }
    }

    public string GUID
    {
        get => _guid;
        set
        {
            if(_guid == null)
                _guid = value;
        }
    }

    public void DealDamage(int ammount) => _behaviourSystem.ReceiveDamage(ammount);


    private void Awake()
    {
        entityData = new DataContainer<EntityData>();
        inventory = new Inventory(this);
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

    private void OnTriggerEnter2D(Collider2D collision) => _behaviourSystem.OnTriggerEnter(collision);

    private void OnTriggerExit2D(Collider2D collision) => _behaviourSystem.OnTriggerLeave(collision);
}
