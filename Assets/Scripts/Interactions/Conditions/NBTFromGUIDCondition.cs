[System.Serializable]
public class NBTFromGUIDCondition : IInteractionCondition
{
    public string GUID;
    public string name;
    public string value;

    public bool CheckCondition()
    {
        GameEntity gameEntity;
        NBTEntityData nbtData;

        if (!LevelManager.SpawnedEntities.TryGetValue(GUID, out gameEntity) || (nbtData = gameEntity.EntityData?.GetData<NBTEntityData>()) == null)
            return false;

        foreach(var nbt in nbtData.Data)
            if (nbt.Key == name && nbt.Value == value)
                return true;

        return false;
    }
}
