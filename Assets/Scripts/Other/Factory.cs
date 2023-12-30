using System;
using UnityEngine;

public static class Factory
{
    public static GameEntity Build(string GUID, GameEntityType entityType, Vector3 position)
    {
        EntityPrefabSO entityPrefab;
        if (!GameDataManager.entityRegistry.TryGetValue(entityType, out entityPrefab))
            return null;

        entityPrefab = UnityEngine.Object.Instantiate(entityPrefab);

        GameObject newEntity = UnityEngine.Object.Instantiate(entityPrefab.prefab, position, Quaternion.identity);
        GameEntity gameEntity = newEntity.AddComponent<GameEntity>();
        gameEntity.GUID = GUID;

        gameEntity.EntityType = Enum.Parse<GameEntityType>(entityPrefab.entityName);
        entityPrefab.entityData.ForEach(data => gameEntity.entityData.AddData(data));

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

        gameEntity.BehaviourSystem = entityPrefab.behaviourSystem;
        gameEntity.MovementSystem = entityPrefab.movementSystem;

        FlashEffect flashEffect = newEntity.AddComponent<FlashEffect>();
        flashEffect.flashMaterial = GameDataManager.resourcesRegistry[("FlashMaterial", typeof(Material))] as Material;
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

        gameItem.itemBehaviour = itemPrefab.itemBehaviour;

        return gameItem;
    }
}