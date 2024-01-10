using System.Linq;

[System.Serializable]
public class NBTFromGameObjectCondition : IInteractionCondition
{
    public GameEntity entity;
    public NBTData data;

    public bool CheckCondition()
    {
        NBTEntityData nbtData;

        if (entity == null || data == null || (nbtData = entity.EntityData?.GetData<NBTEntityData>()) == null)
            return false;

        if (nbtData.Data.FirstOrDefault(x => x.Key == data.Key && x.Value == data.Value) != null)
            return true;

        return false;
    }
}
