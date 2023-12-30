using UnityEngine;

public interface IBehaviourSystem
{
    GameEntity context { get; set; }
    void Update() { }
    void OnTriggerEnter(Collider2D other) { }
    void OnTriggerLeave(Collider2D other) { }
    void ReceiveDamage(int ammount)
    {
        if(context == null) 
            return;

        LivingEntityData livingEntityData = context.GetComponent<GameEntity>()?.entityData.GetData<LivingEntityData>();
        
        if (livingEntityData == null || livingEntityData.isImmortal)
            return;

        livingEntityData.health -= ammount;
        context.GetComponent<FlashEffect>().Flash();
        EventManager.Instance.Publish(new OnEntityDamageEvent(context, ammount));

        if (livingEntityData.health <= 0)
            EventManager.Instance.Publish(new OnEntityDieEvent(context));
    }
    void Dispose();
}