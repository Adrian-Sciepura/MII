using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPrefab", menuName = "ScriptableObjects/ItemPrefab", order = 1)]
public class ItemPrefabSO : ScriptableObject
{
    [SerializeField]
    public string itemName;

    [SerializeField]
    public GameObject itemPrefab;

    [SerializeReference, SubclassSelector]
    public IAttackAction attackAction;

    [SerializeReference, SubclassSelector]
    public IUseAction useAction;

    [SerializeReference, SubclassSelector]
    public List<ItemData> data;
}