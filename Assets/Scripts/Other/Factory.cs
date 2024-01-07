using UnityEngine;

public static class Factory
{
    public static Item Build(ItemType itemType, Vector3 position, int horizontal)
    {
        ItemPrefabSO itemPrefab;
        if(!GameDataManager.itemRegistry.TryGetValue(itemType, out itemPrefab))
            return null;

        itemPrefab = UnityEngine.Object.Instantiate(itemPrefab);
        
        GameObject newObject = UnityEngine.Object.Instantiate(
            itemPrefab.itemPrefab, 
            position + new Vector3(itemPrefab.itemPrefab.transform.position.x * horizontal, itemPrefab.itemPrefab.transform.position.y, 0), 
            Quaternion.identity);
        
        
        newObject.name = itemPrefab.itemName;

        ItemDataContainer itemDataContainer = newObject.AddComponent<ItemDataContainer>();
        
        foreach (var data in itemPrefab.data)
            itemDataContainer.AddData(data);


        Item gameItem = newObject.AddComponent<Item>();
        gameItem.itemBehaviour = itemPrefab.itemBehaviour;

        return gameItem;
    }
}