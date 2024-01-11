using System.Collections;
using UnityEngine;

[System.Serializable]
public class ConsumeItemBehaviour : ItemBehaviour
{
    protected HealingItemData _healingData;
    protected Coroutine _coroutine;

    public override void UpdateContext()
    {
        base.UpdateContext();
        _healingData = GetComponent<ItemDataContainer>().GetData<HealingItemData>();
        Animation = ItemAnimation.Consume;
    }

    public override bool Use()
    {
        if(base.Use())
        {
            _coroutine = StartCoroutine(DestroyAfterUse());
            return true;
        }

        return false;
    }



    protected IEnumerator DestroyAfterUse()
    {
        do
        {
            yield return new WaitForSeconds(1);
        } while (CheckIfInUse());
            

        _coroutine = null;

        if(_healingData != null)
        {
            LivingEntityData livingEntityData = _context.inventory.gameObject.GetComponent<EntityDataContainer>()?.GetData<LivingEntityData>();

            if (livingEntityData != null)
            {
                if (livingEntityData.health + _healingData.health > livingEntityData.maxHealth)
                    livingEntityData.health = livingEntityData.maxHealth;
                else
                    livingEntityData.health += _healingData.health;

                EventManager.Publish(new OnEntityHealEvent(_context.inventory.gameObject.GetComponent<GameEntity>()));
            }
        }
        

        _context.inventory.RemoveItem(_context.type);
    }
}