using UnityEngine;

public abstract class BehaviourSystem : MonoBehaviour
{
    protected virtual void Awake()
    { }
    protected virtual void Update()
    { }
    protected virtual void OnDestroy()
    { }
    protected virtual void OnEnable()
    { }
    protected virtual void OnDisable()
    { }

    public virtual void ReceiveDamage(int ammount)
    {
        GameEntity gameEntity = GetComponent<GameEntity>();
        LivingEntityData livingEntityData = gameEntity.EntityData?.GetData<LivingEntityData>();

        if (livingEntityData == null || livingEntityData.isImmortal)
            return;

        livingEntityData.health -= ammount;
        GetComponent<FlashEffect>()?.Flash();
        EventManager.Publish(new OnEntityDamageEvent(gameEntity, ammount));

        if (livingEntityData.health <= 0)
            EventManager.Publish(new OnEntityDieEvent(gameEntity));
    }
}