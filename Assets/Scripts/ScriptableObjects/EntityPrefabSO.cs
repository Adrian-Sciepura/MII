using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityPrefab", order = 1)]
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
}
