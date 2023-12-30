using UnityEngine;

[System.Serializable]
public class SwordItemBehaviour : IItemBehaviour
{
    public ItemAnimation useAnimation => ItemAnimation.Swing;
    public Item context {  get; set; }

    public void OnCollision(Collider2D collision)
    {
        if (collision.isTrigger)
            return;


        GameEntity entity = collision.GetComponent<GameEntity>();

        if (entity == null || entity == context.inventory.owner)
            return;


        string animation = useAnimation.ToString().ToLower();
        Animator ownerAnimator = context.inventory?.owner.GetComponent<Animator>();
        if (ownerAnimator != null)
        {
            AnimatorStateInfo animatorState = ownerAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animatorState.IsName(animation) || animatorState.normalizedTime >= 1.0f)
                return;
        }



        
        entity.DealDamage(10);
    }

    
}