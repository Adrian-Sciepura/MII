using UnityEngine;

[System.Serializable]
public class RaycastEntityData : EntityData
{
    public RaycastElement groundCheck;
    public RaycastElement wallCheck;
    public RaycastElement fallCheck;
}

[System.Serializable]
public class RaycastElement
{
    public Vector2 offset;
    public Vector2 size;
}