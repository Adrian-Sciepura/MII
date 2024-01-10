using UnityEngine;

public abstract class WalkMovementSystemTemplate : MovementSystem
{
    protected Animator _animator;

    protected LandEntityData _movementData;
    protected Transform _groundCheckTransform;

    protected RaycastEntityData _raycastData;

    protected RaycastElement _groundCheckData;

    protected override void Awake()
    {
        base.Awake();
        EntityDataContainer entityData = GetComponent<EntityDataContainer>();
        
        _animator = GetComponent<Animator>();
        _movementData = entityData.GetData<LandEntityData>();
        _raycastData = entityData.GetData<RaycastEntityData>();
        _groundCheckData = _raycastData.groundCheck;
    }

    protected virtual void Jump()
    {
        if (IsGrounded())
            SetVelocity(_rigidBody.velocity.x, _movementData.jumpForce);
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