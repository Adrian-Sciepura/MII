using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataManager
{
    public static readonly PlayerInputController input = new PlayerInputController();
    public static readonly Dictionary<GameEntityType, EntityPrefabSO> entityRegistry = new Dictionary<GameEntityType, EntityPrefabSO>();
    public static readonly Dictionary<ItemType, ItemPrefabSO> itemRegistry = new Dictionary<ItemType, ItemPrefabSO>();
    public static readonly Dictionary<(string resourceName, Type resourceType), object> resourcesRegistry = new Dictionary<(string, Type), object>();

    public static void InitGameData()
    {
        EntityPrefabSO[] entityPrefabs = Resources.LoadAll<EntityPrefabSO>("Entity");

        foreach (var prefab in entityPrefabs)
        {
            if (prefab.Validate())
            {
                GameEntityType entityType;
                if (Enum.TryParse(prefab.entityName, out entityType) &&
                    !entityRegistry.ContainsKey(entityType))
                {
                    entityRegistry.Add(entityType, prefab);
                }
            }
        }

        ItemPrefabSO[] itemPrefabs = Resources.LoadAll<ItemPrefabSO>("Item");

        foreach (var itemPrefab in itemPrefabs)
        {
            if(itemPrefab.Validate())
            {
                ItemType itemType;
                if(Enum.TryParse(itemPrefab.itemName, out itemType) &&
                    !itemRegistry.ContainsKey(itemType))
                {
                    itemRegistry.Add(itemType, itemPrefab);
                }
            }
        }


        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        
        foreach (var prefab in prefabs)
            resourcesRegistry.Add((prefab.name, typeof(GameObject)), prefab);

        Material[] materials = Resources.LoadAll<Material>("Materials");

        foreach(var material in materials)
            resourcesRegistry.Add((material.name, typeof(Material)), material);

    }
}
