using UnityEngine;

[System.Serializable]
public class SwordItemBehaviour : ItemBehaviour
{
    protected WhiteWeaponItemData _damageInfo;

    public override void UpdateContext()
    {
        base.UpdateContext();
        _damageInfo = GetComponent<ItemDataContainer>().GetData<WhiteWeaponItemData>();
        Animation = ItemAnimation.Swing;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        GameEntity entity = collision.GetComponent<GameEntity>();

        if (entity == null || _context == null || collision.gameObject == _context.inventory.gameObject || !CheckIfInUse())
            return;


        entity.ReceiveDamage(gameObject, _damageInfo.damage);
    }
}