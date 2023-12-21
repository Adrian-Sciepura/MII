using UnityEngine;

[System.Serializable]
public class LandEntityData : EntityData
{
    [SerializeField]
    public int speed;

    public int jumpForce;

    public LayerMask groundLayer;
}