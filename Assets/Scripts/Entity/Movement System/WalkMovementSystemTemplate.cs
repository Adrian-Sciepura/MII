using UnityEngine;

[System.Serializable]
public abstract class WalkMovementSystemTemplate : IMovementSystem
{
    protected GameEntity _context;

    protected Transform _transform;
    protected Rigidbody2D _rigidBody;
    protected Animator _animator;

    protected LandEntityData _movementData;

    protected bool _isFacingRight;
    
    public virtual GameEntity context
    {
        get => _context;
        set
        {
            if (_context != null)
                return;

            _context = value;

            _isFacingRight = true;
            _transform = _context.transform;
            _rigidBody = _context.GetComponent<Rigidbody2D>();
            _animator = _context.GetComponent<Animator>();
            _movementData = _context.entityData.GetData<LandEntityData>();
        }
    }

    public virtual void Dispose()
    {

    }

    public virtual void Update()
    {

    }

    protected virtual void Jump()
    {
        if (IsGrounded()) 
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _movementData.jumpForce);
    }

    protected virtual void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = _transform.localScale;
        localScale.x *= -1;
        _transform.localScale = localScale;
    }

    protected virtual bool IsGrounded()
    {
        return Physics2D.Raycast(_transform.position, Vector2.down, 1.45f, _movementData.groundLayer);
    }
}