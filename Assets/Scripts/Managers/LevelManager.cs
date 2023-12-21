using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        SpawnInfo[] spawnInfos = FindObjectsOfType<SpawnInfo>();

        foreach(SpawnInfo spawnInfo in spawnInfos)
        {
            GameEntityFactory.Build(spawnInfo.entityName, spawnInfo.gameObject.transform.position);
            Destroy(spawnInfo.gameObject);
        }
    }
}
