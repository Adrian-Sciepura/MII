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
    [HideInInspector]
    public Inventory inventory;
    public ItemType type;

    public void Use() =>  GetComponent<ItemBehaviour>()?.Use();
}