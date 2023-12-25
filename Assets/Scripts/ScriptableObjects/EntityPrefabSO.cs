using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrefab", menuName = "ScriptableObjects/EntityPrefab", order = 1)]
public class EntityPrefabSO : ScriptableObject, IAutoGenerated
{
    [SerializeField]
    public string entityName;

    [SerializeField]
    public string displayName;

    [SerializeField]
    public GameObject prefab;

    [SerializeReference, SubclassSelector]
    public List<EntityData> entityData;

    [SerializeReference, SubclassSelector]
    public IBehaviourSystem behaviourSystem;

    [SerializeReference, SubclassSelector]
    public IMovementSystem movementSystem;

    [SerializeField]
    public List<InteractionSystem> interactions;

    /// <summary>
    /// NOTICE - first item in list is default held item
    /// </summary>
    [SerializeField]
    public List<ItemPrefabSO> inventoryItems;


    public string GetGeneratedName()
    {
        return entityName;
    }

    public bool Validate()
    {
        return entityName != null &&
               prefab != null &&
               entityData != null &&
               behaviourSystem != null &&
               movementSystem != null;
    }
}
