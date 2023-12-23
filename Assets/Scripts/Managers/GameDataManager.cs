using System;
using System.Collections.Generic;
using UnityEditor;

public static class GameDataManager
{
    private static readonly string SO_PATH = "Assets/ScriptableObjects";

    public static readonly PlayerInputController input = new PlayerInputController();
    public static readonly Dictionary<GameEntityType, EntityPrefabSO> entityRegistry = new Dictionary<GameEntityType, EntityPrefabSO>();

    public static void InitEntityData()
    {
        string[] assets = AssetDatabase.FindAssets("t:EntityPrefabSO", new[] { SO_PATH + "/Entity" });

        foreach (string asset in assets)
        {
            EntityPrefabSO entityPrefab = AssetDatabase.LoadAssetAtPath<EntityPrefabSO>(AssetDatabase.GUIDToAssetPath(asset));
            if (Validator.Validate(entityPrefab))
            {
                GameEntityType entityType;
                if (Enum.TryParse<GameEntityType>(entityPrefab.entityName, out entityType) &&
                    !entityRegistry.ContainsKey(entityType))
                {
                    entityRegistry.Add(entityType, entityPrefab);
                }
            }
        }
    }
}
