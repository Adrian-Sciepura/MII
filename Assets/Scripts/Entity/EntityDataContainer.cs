using System.Collections.Generic;
using System.Linq;

public class EntityDataContainer
{
    private List<EntityData> _data;

    public EntityDataContainer()
    {
        _data = new List<EntityData>();
    }

    public T GetEntityData<T>() where T : EntityData => _data.FirstOrDefault(data => data is T) as T;

    public bool AddEntityData<T>(EntityData data) where T : EntityData
    {
        if (GetEntityData<T>() != null)
            return false;

        _data.Add(data);
        return true;
    }

    public bool AddEntityData(EntityData data)
    {
        System.Type dataType = data.GetType();
        if(_data.FirstOrDefault(x => x.GetType() == dataType) != null)
            return false;

        _data.Add(data);
        return true;
    }

    public bool RemoveEntityData<T>() where T : EntityData
    {
        EntityData? data = GetEntityData<T>();

        if (data == null)
            return false;

        return _data.Remove(data);
    }
}