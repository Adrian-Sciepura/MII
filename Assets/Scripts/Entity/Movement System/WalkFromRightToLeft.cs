using UnityEngine;

public class WalkFromRightToLeft : WalkMovementSystemTemplate
{
    protected Vector3 _startPos;
    protected Vector3 _doNotFallCheck;
    protected Vector3 _wallCheck;

    protected int _maxWalkDistance;
    protected float _checkDistance;
    protected int Horizontal => _isFacingRight ? 1 : -1;
    protected RaycastElement _wallCheckData;
    protected RaycastElement _fallCheckData;

    protected override void Awake()
    {
        base.Awake();

        _wallCheckData = _raycastData.wallCheck;
        _fallCheckData = _raycastData.fallCheck;
        _startPos = transform.position;
    }

    protected override void Update()
    {
        if (!CheckFallCollider() || CheckWallCollider())
            Flip();

        _rigidBody.velocity = new Vector2(_movementData.speed * Horizontal, _rigidBody.velocity.y);

        _animator.SetBool("isRunning", true);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(Application.isPlaying)
        {
            Gizmos.DrawWireCube(transform.position - transform.up * _fallCheckData.offset.x - transform.right * _fallCheckData.offset.y * Horizontal, _fallCheckData.size);
            Gizmos.DrawWireCube(transform.position - transform.up * _wallCheckData.offset.x - transform.right * _wallCheckData.offset.y * Horizontal, _wallCheckData.size);
        }
    }


    protected bool CheckFallCollider() => Physics2D.BoxCast(transform.position - transform.up * _fallCheckData.offset.x - transform.right * _fallCheckData.offset.y * Horizontal, _fallCheckData.size, 0, Vector2.zero, 0, _movementData.groundLayer);
    protected bool CheckWallCollider() => Physics2D.BoxCast(transform.position - transform.up * _wallCheckData.offset.x - transform.right * _wallCheckData.offset.y * Horizontal, _wallCheckData.size, 0, Vector2.zero, 0, _movementData.groundLayer);

    protected override void Flip()
    {
        base.Flip();
        _doNotFallCheck.x *= -1;
        _wallCheck.x *= -1;
    }
}