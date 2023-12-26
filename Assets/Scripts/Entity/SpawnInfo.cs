using System;
using UnityEngine;

public class SpawnInfo : MonoBehaviour
{
    public GameEntityType entityType;
    public string guid = Guid.NewGuid().ToString();
}