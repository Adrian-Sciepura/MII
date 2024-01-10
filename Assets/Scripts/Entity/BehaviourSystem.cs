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

    public virtual void ReceiveDamage(GameObject sender, int ammount)
    {
        GameEntity gameEntity = GetComponent<GameEntity>();
        LivingEntityData livingEntityData = gameEntity.EntityData?.GetData<LivingEntityData>();

        if (livingEntityData == null)
            return;

        if (!livingEntityData.isKnockbackResistant)
            AddKnockback(sender, 8);

        if (!livingEntityData.isImmortal)
        {
            livingEntityData.health -= ammount;
            GetComponent<FlashEffect>()?.Flash();
            EventManager.Publish(new OnEntityDamageEvent(gameEntity, ammount));

            if (livingEntityData.health <= 0)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                EventManager.Publish(new OnEntityDieEvent(gameEntity));
            }
        }
    }

    public virtual void AddKnockback(GameObject sender, int strength)
    {
        Vector2 knockbackForce = (transform.position - sender.transform.position).normalized;
        knockbackForce.y = 0;
        knockbackForce.x *= strength;
        GetComponent<MovementSystem>()?.AddKnockback(knockbackForce);
    }
}