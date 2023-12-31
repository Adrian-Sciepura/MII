using UnityEngine;

public enum ItemAnimation
{
    None,
    Swing,
    Consume,
    Shoot
}

public class Item : MonoBehaviour
{
    private IItemBehaviour _itemBehaviour;

    public readonly DataContainer<ItemData> itemData = new DataContainer<ItemData>();
    public Inventory inventory;
    public ItemType type;

    public IItemBehaviour itemBehaviour
    {
        set
        {
            if (_itemBehaviour != null)
                _itemBehaviour.context = null;

            _itemBehaviour = value;
            _itemBehaviour.context = this;
        }
    }
    
    public void Use() => _itemBehaviour.Use();
    private void OnTriggerEnter2D(Collider2D collision) => _itemBehaviour.OnCollision(collision);
}