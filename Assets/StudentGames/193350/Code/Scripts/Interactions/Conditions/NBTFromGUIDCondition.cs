using System.Linq;

[System.Serializable]
public class NBTFromGUIDCondition : IInteractionCondition
{
    public string GUID;
    public NBTData data;

    public bool CheckCondition()
    {
        GameEntity gameEntity;
        NBTEntityData nbtData;

        if (GUID == string.Empty || 
            data == null ||
            !LevelManager.SpawnedEntities.TryGetValue(GUID, out gameEntity) || 
            (nbtData = gameEntity.EntityData?.GetData<NBTEntityData>()) == null)
            return false;

        if(nbtData.Data.FirstOrDefault(x => x.Key == data.Key && x.Value == data.Value) != null)
            return true;

        return false;
    }
}
