using UnityEngine;

[System.Serializable]
public abstract class ItemBehaviour : MonoBehaviour
{
    protected ItemAnimation _animation { get; private set; }
    protected string _animationName { get; private set; }

    protected Item _context;
    protected Animator _ownerAnimator;
    

    protected ItemAnimation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            _animationName = _animation.ToString().ToLower();
        }
    }

    public virtual void UpdateContext()
    {
        _context = GetComponent<Item>();
        _ownerAnimator = _context.inventory.gameObject.GetComponent<Animator>();
    }

    protected virtual void OnDestroy()
    { }

    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    { }

    public virtual bool Use()
    {
        if (CheckIfInUse())
            return false;

        _ownerAnimator.SetTrigger(_animationName);
        return true;
    }

    public virtual void StopUsing()
    {
        _ownerAnimator.SetTrigger("abort");
    }

    protected bool CheckIfInUse()
    {
        if (_ownerAnimator == null)
            return false;

        AnimatorStateInfo animatorState = _ownerAnimator.GetCurrentAnimatorStateInfo(1);
        if (animatorState.IsName(_animationName))
            return true;

        return false;
    }
}