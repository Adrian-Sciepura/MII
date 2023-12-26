using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static readonly List<GameEntity> SpawnedEntities = new List<GameEntity>();
    public static GameEntity playerEntity { get; private set; }

    public static void Setup()
    {
        EventManager.Subscribe<OnHighPriorityLevelLoadEvent>(SetupEntitiesOnScene);
    }

    public static GameEntity FindNearest(GameEntity entity, List<GameEntity> entityList = null)
    {
        List<GameEntity> listToSearch = entityList ?? SpawnedEntities;

        Vector3 position = entity.transform.position;

        float nearestDistance = 99999f;
        GameEntity currentNearest = null;

        foreach (var spawnedEntity in listToSearch)
        {
            if (spawnedEntity != entity)
            {
                float distance = Vector3.Distance(position, spawnedEntity.gameObject.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    currentNearest = spawnedEntity;
                }
            }
        }

        return currentNearest;
    }

    public static GameEntity FindNearest(GameEntity entity, GameEntityType search, List<GameEntity> entityList = null)
    {
        List<GameEntity> listToSearch = entityList ?? SpawnedEntities;

        Vector3 position = entity.transform.position;

        float nearestDistance = 99999f;
        GameEntity currentNearest = null;

        foreach (var spawnedEntity in listToSearch)
        {
            if (spawnedEntity.entityType == search && spawnedEntity != entity)
            {
                float distance = Vector3.Distance(position, spawnedEntity.gameObject.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    currentNearest = spawnedEntity;
                }
            }
        }

        return currentNearest;
    }

    private static void SetupEntitiesOnScene(OnHighPriorityLevelLoadEvent e)
    {
        SpawnInfo[] spawnInfos = Object.FindObjectsOfType<SpawnInfo>();

        foreach (SpawnInfo spawnInfo in spawnInfos)
        {
            GameEntity createdEntity = Factory.Build(spawnInfo.entityType, spawnInfo.gameObject.transform.position);
            SpawnedEntities.Add(createdEntity);

            if (spawnInfo.entityType == GameEntityType.Player)
                playerEntity = createdEntity;

            Object.Destroy(spawnInfo.gameObject);
        }
    }
}
