using UnityEngine;

public static class Factory
{
    public static GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation, int horizontal)
    {
        GameObject newObject = UnityEngine.Object.Instantiate(
            prefab,
            position + new Vector3(prefab.transform.position.x * horizontal, prefab.transform.position.y, 0),
            prefab.transform.rotation * rotation);

        return newObject;
    }

    public static Item Build(ItemType itemType, Vector3 position, Quaternion rotation, int horizontal)
    {
        ItemPrefabSO itemPrefab;
        if (!GameDataManager.itemRegistry.TryGetValue(itemType, out itemPrefab))
            return null;

        itemPrefab = UnityEngine.Object.Instantiate(itemPrefab);

        GameObject newObject = SpawnPrefab(itemPrefab.itemPrefab, position, rotation, horizontal);

        newObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        newObject.name = itemPrefab.itemName;

        ItemDataContainer itemDataContainer = newObject.AddComponent<ItemDataContainer>();

        foreach (var data in itemPrefab.itemData)
            itemDataContainer.AddData(data);


        Item gameItem = newObject.AddComponent<Item>();
        gameItem.type = itemType;

        return gameItem;
    }
}