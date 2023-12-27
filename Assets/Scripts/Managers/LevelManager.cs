using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static readonly Dictionary<string, GameEntity> spawnedEntities = new Dictionary<string, GameEntity>();
    public static GameEntity playerEntity { get; private set; }

    public static void Setup()
    {
        EventManager.Subscribe<OnHighPriorityLevelLoadEvent>(SetupEntitiesOnScene);
    }

    private static void SetupEntitiesOnScene(OnHighPriorityLevelLoadEvent e)
    {
        spawnedEntities.Clear();

        SpawnInfo[] spawnInfos = Object.FindObjectsOfType<SpawnInfo>();

        foreach (SpawnInfo spawnInfo in spawnInfos)
        {
            GameEntity createdEntity = Factory.Build(spawnInfo.entityType, spawnInfo.gameObject.transform.position);
            spawnedEntities.Add(spawnInfo.guid, createdEntity);

            if (spawnInfo.entityType == GameEntityType.Player)
                playerEntity = createdEntity;

            for(int i = spawnInfo.transform.childCount - 1; i >= 0 ; i--)
                spawnInfo.transform.GetChild(i).transform.parent = createdEntity.transform;

            Object.Destroy(spawnInfo.gameObject);
        }
    }
}
