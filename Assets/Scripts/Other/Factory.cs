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

        GameObject handObject = newEntity.transform.Find("hand").gameObject;
        if(handObject != null)
        {
            foreach (ItemType itemType in entityPrefab.inventoryItems)
            {
                Item item = Build(itemType, handObject.transform.position);

                if (item == null)
                    continue;

                item.transform.parent = handObject.transform;
                item.gameObject.SetActive(false);
                gameEntity.inventory.AddItem(item);
            }

            if (gameEntity.inventory.items[0] != null)
                gameEntity.inventory.items[0].gameObject.SetActive(true);
        }

        return gameEntity;
    }

    public static Item Build(ItemType itemType, Vector3 position)
    {
        ItemPrefabSO itemPrefab;
        if(!GameDataManager.itemRegistry.TryGetValue(itemType, out itemPrefab))
            return null;

        GameObject newObject = UnityEngine.Object.Instantiate(itemPrefab.itemPrefab, position + itemPrefab.itemPrefab.transform.position, Quaternion.identity);
        Item gameItem = newObject.AddComponent<Item>();

        foreach (var data in itemPrefab.data)
            gameItem.itemData.AddData(data);

        gameItem.useAction = itemPrefab.useAction;

        return gameItem;
    }
}