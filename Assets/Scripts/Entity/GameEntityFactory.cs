using UnityEngine;

public static class GameEntityFactory
{
    public static GameObject Build(string name, Vector3 position)
    {
        EntityPrefabSO entityPrefab;
        if (!GameDataManager.entityRegistry.TryGetValue(name, out entityPrefab))
            return null;

        GameObject newEntity = Object.Instantiate(entityPrefab.prefab, position, Quaternion.identity);
        GameEntity gameEntity = newEntity.AddComponent<GameEntity>();

        foreach(EntityData data in entityPrefab.entityData)
            gameEntity.entityData.AddData(data);

        gameEntity.BehaviourSystem = entityPrefab.behaviourSystem;
        gameEntity.MovementSystem = entityPrefab.movementSystem;
        return newEntity;
    }
}