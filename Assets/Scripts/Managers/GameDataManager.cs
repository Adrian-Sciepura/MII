using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataManager
{
    public static readonly PlayerInputController input = new PlayerInputController();
    public static readonly Dictionary<GameEntityType, EntityPrefabSO> entityRegistry = new Dictionary<GameEntityType, EntityPrefabSO>();
    public static readonly Dictionary<string, GameObject> prefabRegistry = new Dictionary<string, GameObject>();
    public static void InitGameData()
    {
        EntityPrefabSO[] prefabs = Resources.LoadAll<EntityPrefabSO>("Entity");

        foreach (var prefab in prefabs)
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


        GameObject[] canvasPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        
        foreach (var canvas in canvasPrefabs)
            prefabRegistry.Add(canvas.name, canvas);
    }
}
