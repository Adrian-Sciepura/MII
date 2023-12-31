using UnityEngine;

[System.Serializable]
public class DoNothingItemBehaviour : IItemBehaviour
{
    public ItemAnimation useAnimation => ItemAnimation.None;

    public Item context { get; set; }
}
