using UnityEngine;

public abstract class WalkMovementSystemTemplate : MovementSystem
{
    protected Rigidbody2D _rigidBody;
    protected Animator _animator;

    protected LandEntityData _movementData;
    protected Transform _groundCheckTransform;

    protected bool _isFacingRight;
    protected RaycastEntityData _raycastData;

    protected RaycastElement _groundCheckData;

    protected override void Awake()
    {
        _isFacingRight = true;
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        EntityDataContainer entityData = GetComponent<EntityDataContainer>();
        _movementData = entityData.GetData<LandEntityData>();
        _raycastData = entityData.GetData<RaycastEntityData>();
        _groundCheckData = _raycastData.groundCheck;
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

    protected override void OnDrawGizmos()
    {
        if(Application.isPlaying)
            Gizmos.DrawWireCube(transform.position - transform.up * _groundCheckData.offset.x - transform.right * _groundCheckData.offset.y, _groundCheckData.size);

    }

    protected virtual bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position - transform.up * _groundCheckData.offset.x - transform.right * _groundCheckData.offset.y, _groundCheckData.size, 0, Vector2.zero, 0, _movementData.groundLayer);
    }
}