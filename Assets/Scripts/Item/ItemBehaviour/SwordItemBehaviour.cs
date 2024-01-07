using UnityEngine;

[System.Serializable]
public class SwordItemBehaviour : IItemBehaviour
{
    private Item _context;
    private WhiteWeaponItemData _damageInfo;
    
    public ItemAnimation useAnimation => ItemAnimation.Swing;
    public Item context 
    {
        get => _context;
        set
        {
            if (_context != null)
                return;

            _context = value;
            _damageInfo = _context.GetComponent<ItemDataContainer>().GetData<WhiteWeaponItemData>();
        }
    }

    public void OnCollision(Collider2D collision)
    {
        if (collision.isTrigger)
            return;


        GameEntity entity = collision.GetComponent<GameEntity>();

        if (_context == null)
            Debug.Log("null context");
        else if (_context.inventory == null)
            Debug.Log("null inventory");



        if (entity == null || collision.gameObject == _context.inventory.gameObject)
            return;


        string animation = useAnimation.ToString().ToLower();
        Animator ownerAnimator = _context.inventory.gameObject.GetComponent<Animator>();
        
        if (ownerAnimator != null)
        {
            AnimatorStateInfo animatorState = ownerAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animatorState.IsName(animation) || animatorState.normalizedTime >= 1.0f)
                return;
        }


        entity.ReceiveDamage(_damageInfo.damage);
    }
}