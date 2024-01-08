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
        Animator ownerAnimator = context.inventory.gameObject.GetComponent<Animator>();
        if(ownerAnimator != null)
        {
            AnimatorStateInfo animatorState = ownerAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(animation) && animatorState.normalizedTime < 1.0f)
                return;

            ownerAnimator.SetTrigger(animation);
        }
    }
    void OnTriggerEnter2D(Collider2D collision){ }
}