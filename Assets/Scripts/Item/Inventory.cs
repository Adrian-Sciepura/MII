using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _maxSize;

    [SerializeField]
    private ItemType[] _items;

    private int _itemsCount;


    public int MaxSize => _maxSize;
    public ItemType[] Items => _items;


    private void Awake()
    {
        if(_maxSize < _items.Length)
            _maxSize = _items.Length;
        else
        {
            ItemType[] newItemsArray = new ItemType[_maxSize];
            for(int i = 0; i < _items.Length; i++)
                newItemsArray[i] = _items[i];

            _items = newItemsArray;
        }

        _itemsCount = _items.Count(x => x != ItemType.NONE);
    }
    
    public Item TakeTheItemInHand(GameObject hand, int slot)
    {
        if (slot < 0 || slot > _maxSize - 1)
            return null;

        if (_items[slot] == ItemType.NONE)
        {
            DestroyCurrent();
            return null;
        }

        DestroyCurrent();

        int horizontal = gameObject.transform.localScale.x > 0 ? 1 : -1;

        Item item = Factory.Build(_items[slot], hand.transform.position, hand.transform.rotation, horizontal);
        item.inventory = this;
        item.GetComponent<ItemBehaviour>().UpdateContext();
        item.transform.parent = hand.transform;
        item.transform.localScale = new Vector2(horizontal, item.transform.localScale.y);
        return item;

        void DestroyCurrent()
        {
            Item currentHeldItem = hand.GetComponentInChildren<Item>();
            if (currentHeldItem != null)
                Destroy(currentHeldItem.gameObject);
        }
    }


    public void Resize(int newSize)
    {
        if (newSize < _itemsCount)
            return;

        ItemType[] newItems = new ItemType[newSize];

        int addedCount = 0;

        foreach (ItemType item in _items)
            if (item != ItemType.NONE)
                newItems[addedCount++] = item;

        _maxSize = newSize;
        _items = newItems;
    }

    public void AddItem(ItemType item, int slot = -1)
    {
        if (_itemsCount >= _maxSize)
            return;

        if (slot < 0 || slot > _maxSize - 1 || _items[slot] != ItemType.NONE)
        {
            for (int i = 0; i < _maxSize; i++)
            {
                if (_items[i] == ItemType.NONE)
                {
                    _items[i] = item;
                    break;
                }
            }
        }
        else
        {
            _items[slot] = item;
        }

        GameEntity gameEntity = GetComponent<GameEntity>();
        if (gameEntity != null)
            EventManager.Publish(new OnEntityPickupItemEvent(gameEntity, item));

        _itemsCount++;
    }

    public void RemoveItem(ItemType item)
    {
        for (int i = 0; i < _maxSize; i++)
        {
            if (_items[i].Equals(item))
            {
                _items[i] = ItemType.NONE;
                RemoveItemFromHand();
                _itemsCount--;
                return;
            }
        }
    }

    public void RemoveItem(int slot)
    {
        if (slot < 0 || slot > _maxSize - 1)
            return;

        _items[slot] = ItemType.NONE;
        _itemsCount--;
        RemoveItemFromHand();
    }

    private void RemoveItemFromHand()
    {
        GameEntity gameEntity = gameObject.GetComponent<GameEntity>();
        
        if(gameEntity != null)
        {
            int currentSlot = gameEntity.HeldItemInventorySlot;
            gameEntity.HeldItemInventorySlot = currentSlot;
        }
    }
}