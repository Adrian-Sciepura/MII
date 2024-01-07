using UnityEngine;

public abstract class WalkMovementSystemTemplate : MovementSystem
{
    protected Rigidbody2D _rigidBody;
    protected Animator _animator;

    protected LandEntityData _movementData;

    protected bool _isFacingRight;

    protected override void Awake()
    {
        _isFacingRight = true;
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _movementData = GetComponent<GameEntity>().EntityData.GetData<LandEntityData>();
    }

    protected virtual void Jump()
    {
        if (IsGrounded()) 
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _movementData.jumpForce);
    }

    protected virtual void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    protected virtual bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.45f, _movementData.groundLayer);
    }
}