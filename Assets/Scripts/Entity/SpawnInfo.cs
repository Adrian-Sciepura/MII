using System;
using Unity.Collections;
using UnityEngine;

public class SpawnInfo : MonoBehaviour
{
    public GameEntityType entityType;
    public string guid = Guid.NewGuid().ToString();
}