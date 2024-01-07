using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameEntity _playerEntity;
    private Dictionary<string, GameEntity> _spawnedEntities;
    private GameObject _spawnEntityParent;

    private static LevelManager _instance;

    public static Dictionary<string, GameEntity> SpawnedEntities => _instance._spawnedEntities;
    public static GameEntity PlayerEntity => _instance._playerEntity;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _spawnedEntities = new Dictionary<string, GameEntity>();

        EventManager.Subscribe<OnEntityDieEvent>(EntityDeath);
        EventManager.Subscribe<OnInteractionItemStartEvent<SetNBTFromGUIDInteractionItem>>(SetNBTFromGUID);
        EventManager.Subscribe<OnInteractionItemStartEvent<SpawnEntityInteractionItem>>(BuildFromPrefab);
        EventManager.Subscribe<OnInteractionItemStartEvent<RemoveGameObjectInteractionItem>>(RemoveGameObject);
    }

    private void Start()
    {
        InteractionTrigger[] triggers = FindObjectsOfType<InteractionTrigger>();

        foreach (InteractionTrigger trigger in triggers)
            trigger.gameObject.tag = "InteractionTrigger";


        _spawnEntityParent = GameObject.Find("Entity");

        _spawnedEntities.Clear();
        GameEntity[] gameEntities = FindObjectsOfType<GameEntity>();
        foreach (var entity in gameEntities)
            _spawnedEntities.Add(entity.GUID, entity);
    }

    private void RemoveGameObject(OnInteractionItemStartEvent<RemoveGameObjectInteractionItem> onRemoveGameObjectInteractionStarted)
    {
        GameObject removeObject = onRemoveGameObjectInteractionStarted.Data.gameObjectToRemove;
        GameEntity gameEntity;
        
        if((gameEntity = removeObject.GetComponent<GameEntity>()) != null && _instance._spawnedEntities.TryGetValue(gameEntity.GUID, out gameEntity))
            _instance._spawnedEntities.Remove(gameEntity.GUID);

        Destroy(removeObject);
        EventManager.Publish(new OnInteractionItemFinishEvent<RemoveGameObjectInteractionItem>());
    }


    private void BuildFromPrefab(OnInteractionItemStartEvent<SpawnEntityInteractionItem> onSpawnEntityInteractionStarted)
    {
        SpawnInfo spawnInfo = onSpawnEntityInteractionStarted.Data.spawnObject.GetComponent<SpawnInfo>();
        if(spawnInfo != null)
        {
            GameObject newObject = Instantiate(spawnInfo.prefab, spawnInfo.gameObject.transform.position, Quaternion.identity);

            for (int i = spawnInfo.transform.childCount - 1; i >= 0; i--)
                spawnInfo.transform.GetChild(i).transform.parent = newObject.transform;

            GameEntity entityComponent = newObject.GetComponent<GameEntity>();

            if (entityComponent != null)
            {
                newObject.transform.parent = _instance._spawnEntityParent.transform;
                SpawnedEntities.Add(entityComponent.GUID, entityComponent);
            }
        }


        EventManager.Publish(new OnInteractionItemFinishEvent<SpawnEntityInteractionItem>());
    }

    private void SetNBTFromGUID(OnInteractionItemStartEvent<SetNBTFromGUIDInteractionItem> onSetNBTInteractionStarted)
    {
        GameEntity gameEntity;
        if (SpawnedEntities.TryGetValue(onSetNBTInteractionStarted.Data.GUID, out gameEntity) && gameEntity.EntityData != null)
        {
            NBTEntityData nbtData = gameEntity.EntityData.GetData<NBTEntityData>();
            if (nbtData == null)
            {
                gameEntity.EntityData.AddData<NBTEntityData>(new NBTEntityData());
                nbtData = gameEntity.EntityData.GetData<NBTEntityData>();
            }


            bool found = false;
            for (int i = 0; i < nbtData.Data.Count; i++)
            {
                if (nbtData.Data[i].Key == onSetNBTInteractionStarted.Data.name)
                {
                    found = true;
                    nbtData.Data[i].Value = onSetNBTInteractionStarted.Data.value;
                    break;
                }
            }

            if(!found)
            {
                NBTData newNBTData = new NBTData();
                newNBTData.Key = onSetNBTInteractionStarted.Data.name;
                newNBTData.Value = onSetNBTInteractionStarted.Data.value;
                nbtData.Data.Add(newNBTData);
            }
                
        }

        EventManager.Publish(new OnInteractionItemFinishEvent<SetNBTFromGUIDInteractionItem>());
    }

    private void EntityDeath(OnEntityDieEvent entityDieEvent)
    {
        SpawnedEntities[entityDieEvent.Entity.GUID] = null;
        Destroy(entityDieEvent.Entity.gameObject);

        Debug.Log("Entity died");
    }
}
