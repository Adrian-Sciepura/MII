using System;
using UnityEngine;

public class Factory
{
    public static GameEntity Build(GameEntityType entityType, Vector3 position)
    {
        EntityPrefabSO entityPrefab;
        if (!GameDataManager.entityRegistry.TryGetValue(entityType, out entityPrefab))
            return null;

        GameObject newEntity = UnityEngine.Object.Instantiate(entityPrefab.prefab, position, Quaternion.identity);
        GameEntity gameEntity = newEntity.AddComponent<GameEntity>();

        foreach (EntityData data in entityPrefab.entityData)
            gameEntity.entityData.AddData(data);

        gameEntity.BehaviourSystem = entityPrefab.behaviourSystem;
        gameEntity.MovementSystem = entityPrefab.movementSystem;

        gameEntity.entityType = Enum.Parse<GameEntityType>(entityPrefab.entityName);

        return gameEntity;
    }
}