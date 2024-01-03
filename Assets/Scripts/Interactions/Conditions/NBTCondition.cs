[System.Serializable]
public class NBTCondition : IInteractionCondition
{
    public string GUID;
    public string name;
    public string value;

    public bool CheckCondition()
    {
        GameEntity gameEntity;
        
        if (!LevelManager.spawnedEntities.TryGetValue(GUID, out gameEntity))
            return false;

        foreach(var nbt in gameEntity.NBT)
            if (nbt.Item1 == name && nbt.Item2 == value)
                return true;

        return false;
    }
}
