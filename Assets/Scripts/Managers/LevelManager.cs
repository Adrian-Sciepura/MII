using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
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
        EventManager.Subscribe<OnInteractionItemStartEvent<ChangeActiveStateInteractionItem>>(ChangeActiveState);
        EventManager.Subscribe<OnInteractionItemStartEvent<SetNBTFromGameObjectInteractionItem>>(SetNBTFromGameObject);
        EventManager.Subscribe<OnInteractionItemStartEvent<ChangePlayerInputActiveStateInteractionItem>>(ChangePlayerInputActiveState);
    }

    private void Start()
    {
        InteractionTrigger[] triggers = FindObjectsOfType<InteractionTrigger>();

        LayerMask bonusMask = LayerMask.NameToLayer("Bonus");
        foreach (InteractionTrigger trigger in triggers)
        {
            trigger.gameObject.tag = "InteractionTrigger";
            trigger.gameObject.layer = bonusMask;
        }


        _spawnEntityParent = GameObject.Find("Entity");

        _spawnedEntities.Clear();
        GameEntity[] gameEntities = FindObjectsOfType<GameEntity>();
        foreach (var entity in gameEntities)
            _spawnedEntities.Add(entity.GUID, entity);
    }


    #region Events

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
                nbtData = new NBTEntityData();
                gameEntity.EntityData.AddData(nbtData);
            }

            NBTData setData = onSetNBTInteractionStarted.Data.data;
            NBTData search = nbtData.Data.FirstOrDefault(x => x.Key == setData.Key);

            if (search != null)
            {
                search.Value = setData.Value;
            }
            else
            {
                NBTData newData = new NBTData();
                newData.Key = setData.Key;
                newData.Value = setData.Value;

                nbtData.Data.Add(newData);
            }
        }

        EventManager.Publish(new OnInteractionItemFinishEvent<SetNBTFromGUIDInteractionItem>());
    }

    private void SetNBTFromGameObject(OnInteractionItemStartEvent<SetNBTFromGameObjectInteractionItem> onSetNBTInteractionStarted)
    {
        NBTData setData = onSetNBTInteractionStarted.Data.data;
        EntityDataContainer entityData = onSetNBTInteractionStarted.Data.entity?.EntityData;

        if(entityData != null)
        {
            NBTEntityData nbtData = entityData.GetData<NBTEntityData>();
            if (nbtData == null)
            {
                nbtData = new NBTEntityData();
                entityData.AddData(nbtData);
            }

            NBTData search = nbtData.Data.FirstOrDefault(x => x.Key == setData.Key);

            if (search != null)
            {
                search.Value = setData.Value;
            }
            else
            {
                NBTData newData = new NBTData();
                newData.Key = setData.Key;
                newData.Value = setData.Value;

                nbtData.Data.Add(newData);
            }
        }

        EventManager.Publish(new OnInteractionItemFinishEvent<SetNBTFromGameObjectInteractionItem>());
    }

    private void ChangeActiveState(OnInteractionItemStartEvent<ChangeActiveStateInteractionItem> onChangeStateInteractionStarted)
    {
        GameObject objectToChange = onChangeStateInteractionStarted.Data.gameObject;

        if (objectToChange != null)
            objectToChange.SetActive(onChangeStateInteractionStarted.Data.active);

        EventManager.Publish(new OnInteractionItemFinishEvent<ChangeActiveStateInteractionItem>());
    }

    private void ChangePlayerInputActiveState(OnInteractionItemStartEvent<ChangePlayerInputActiveStateInteractionItem> onChangePlayerInputStateStarted)
    {
        if (onChangePlayerInputStateStarted.Data.active)
            GameDataManager.input.Player.Enable();
        else
            GameDataManager.input.Player.Disable();

        EventManager.Publish(new OnInteractionItemFinishEvent<ChangePlayerInputActiveStateInteractionItem>());
    }

    private void EntityDeath(OnEntityDieEvent entityDieEvent)
    {
        SpawnedEntities[entityDieEvent.Entity.GUID] = null;
        Destroy(entityDieEvent.Entity.gameObject);

        Debug.Log("Entity died");
    }

    #endregion
}
