using UnityEngine;

public static class LevelManager
{
    public static void Setup()
    {
        EventManager.Subscribe<OnLevelLoadEvent>(SetupEntitiesOnScene);
    }

    private static void SetupEntitiesOnScene(OnLevelLoadEvent e)
    {
        SpawnInfo[] spawnInfos = Object.FindObjectsOfType<SpawnInfo>();

        foreach(SpawnInfo spawnInfo in spawnInfos)
        {
            Factory.Build(spawnInfo.entityType, spawnInfo.gameObject.transform.position);
            Object.Destroy(spawnInfo.gameObject);
        }
    }
}
