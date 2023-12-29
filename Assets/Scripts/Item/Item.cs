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
    public readonly DataContainer<ItemData> itemData = new DataContainer<ItemData>();
    public IUseAction useAction;
    public ItemType type;
}