using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrefab", menuName = "ScriptableObjects/EntityPrefab", order = 1)]
public class EntityPrefabSO : ScriptableObject
{
    [SerializeField]
    public string entityName;

    [SerializeField]
    public GameObject prefab;

    [SerializeReference, SubclassSelector]
    public List<EntityData> entityData;

    [SerializeReference, SubclassSelector]
    public IBehaviourSystem behaviourSystem;

    [SerializeReference, SubclassSelector]
    public IMovementSystem movementSystem;

    /// <summary>
    /// NOTICE - first item in list is default held item
    /// </summary>
    [SerializeField]
    public List<ItemPrefabSO> inventoryItems;
}
