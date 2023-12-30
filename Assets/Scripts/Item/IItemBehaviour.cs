using UnityEngine;

public interface IItemBehaviour
{
    ItemAnimation useAnimation { get; }
    Item context { get; set; }
    void Use()
    {
        if (context == null)
            return;

        string animation = useAnimation.ToString().ToLower();
        Animator ownerAnimator = context.inventory?.owner.GetComponent<Animator>();
        if(ownerAnimator != null)
        {
            AnimatorStateInfo animatorState = ownerAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(animation) && animatorState.normalizedTime < 1.0f)
                return;

            ownerAnimator.SetTrigger(animation);
        }
    }
    void OnCollision(Collider2D collision) { }
}