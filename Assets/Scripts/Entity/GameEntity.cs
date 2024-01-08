using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private GameEntityCategory _entityCategory;
    private Inventory _inventory;
    private EntityDataContainer _entityData;
    private BehaviourSystem _behaviourSystem;
    private MovementSystem _movementSystem;
    private GameObject _handObject;
    private Item _heldItem;
    private int _heldItemInventorySlot = 0;
    private string _guid;

    public string GUID => _guid;
    public GameEntityCategory EntityCategory => _entityCategory;
    public Inventory Inventory
    {
        get
        {
            if(_inventory == null)
                _inventory = GetComponent<Inventory>();

            return _inventory;
        }
    }
    public EntityDataContainer EntityData
    {
        get
        {
            if(_entityData == null)
                _entityData = GetComponent<EntityDataContainer>();

            return _entityData;
        }
    }

    public BehaviourSystem BehaviourSystem
    {
        get
        {
            if (_behaviourSystem == null)
                _behaviourSystem = GetComponents<BehaviourSystem>().FirstOrDefault(x => x.enabled);
            
            return _behaviourSystem;
        }
    }

    public MovementSystem MovementSystem
    {
        get
        {
            if(_movementSystem == null)
                _movementSystem = GetComponents<MovementSystem>().FirstOrDefault(x => x.enabled);

            return _movementSystem;
        }
    }

    public int HeldItemInventorySlot
    {
        get => _heldItemInventorySlot;
        set
        {
            if(Inventory != null && value >= 0 && value < _inventory.MaxSize)
            {
                int prevValue = _heldItemInventorySlot;
                _heldItemInventorySlot = value;
                _heldItem = _inventory.TakeTheItemInHand(_handObject, value);
                EventManager.Publish(new OnEntityChangeHeldItemEvent(this, prevValue));
            }
        }
    }

    public Item HeldItem => _heldItem;


    public void SwitchMovementSystem<T>() where T : MovementSystem
    {
        if (MovementSystem is T)
            return;

        T newComponent = GetComponent<T>();
        if (newComponent != null)
        {
            _movementSystem.enabled = false;
            _movementSystem = newComponent;
            newComponent.enabled = true;
        }
    }

    public void SwitchBehaviourSystem<T>() where T : BehaviourSystem
    {
        if(BehaviourSystem is T)
            return;

        T newComponent = GetComponent<T>();
        if(newComponent != null)
        {
            _behaviourSystem.enabled = false;
            _behaviourSystem = newComponent;
            newComponent.enabled = true;
        }
    }

    public void ReceiveDamage(int ammount) => BehaviourSystem.ReceiveDamage(ammount);

    private void Awake()
    {
        _guid = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        _handObject = transform.Find("hand").gameObject;
        HeldItemInventorySlot = 0;
    }
}
