using System.Collections.Generic;

[System.Serializable]
public class NBTEntityData : EntityData
{
    public List<NBTData> Data;
}

[System.Serializable]
public class NBTData
{
    public string Key;
    public string Value;
}