using UnityEngine;

[System.Serializable]
public class LandEntityData : EntityData
{
    public int speed;

    public int jumpForce;

    public LayerMask groundLayer;
}