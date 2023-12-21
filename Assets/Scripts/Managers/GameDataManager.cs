using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GameDataManager
{
    private static readonly string SO_PATH = "Assets/ScriptableObjects";

    public static readonly PlayerInputController input = new PlayerInputController();
    public static readonly Dictionary<string, EntityPrefabSO> entityRegistry = new Dictionary<string, EntityPrefabSO>();

    public static void InitEntityData()
    {
        string[] assets = AssetDatabase.FindAssets("t:EntityPrefabSO", new[] { SO_PATH + "/Entity" });

        foreach (string asset in assets)
        {
            EntityPrefabSO entityPrefab = AssetDatabase.LoadAssetAtPath<EntityPrefabSO>(AssetDatabase.GUIDToAssetPath(asset));
            if (entityPrefab != null && entityPrefab.entityName != null && !entityRegistry.ContainsKey(entityPrefab.entityName))
                if (entityPrefab != null && entityPrefab.entityName != null && !entityRegistry.ContainsKey(entityPrefab.entityName))
                    entityRegistry.Add(entityPrefab.entityName, entityPrefab);
        }
    }
}
