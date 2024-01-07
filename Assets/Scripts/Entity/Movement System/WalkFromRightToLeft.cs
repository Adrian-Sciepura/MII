using UnityEngine;

public class WalkFromRightToLeft : WalkMovementSystemTemplate
{
    protected Vector3 _startPos;
    protected Vector3 _doNotFallCheck;
    protected Vector3 _wallCheck;

    protected int _maxWalkDistance;
    protected float _checkDistance;
    protected int Horizontal => _isFacingRight ? 1 : -1;


    protected override void Awake()
    {
        base.Awake();

        Collider2D mainCollider = GetComponent<Collider2D>();

        if (mainCollider != null)
        {
            _checkDistance = 0.4f;
            _doNotFallCheck = new Vector3(1.0f, -mainCollider.bounds.size.y * 0.7f - 0.05f, 0);
            _wallCheck = new Vector3(mainCollider.bounds.size.x / 2 + 0.05f, -0.5f, 0);
        }

        _startPos = transform.position;
    }

    protected override void Update()
    {
        if (!CheckFallCollider() || CheckWallCollider())
            Flip();

        _rigidBody.velocity = new Vector2(_movementData.speed * Horizontal, _rigidBody.velocity.y);

        _animator.SetBool("isRunning", true);
    }

    protected bool CheckFallCollider() => Physics2D.Raycast(transform.position + _doNotFallCheck, Vector2.down, _checkDistance, _movementData.groundLayer);
    protected bool CheckWallCollider() => Physics2D.Raycast(transform.position + _wallCheck, Vector2.right * Horizontal, _checkDistance, _movementData.groundLayer);

    protected override void Flip()
    {
        base.Flip();
        _doNotFallCheck.x *= -1;
        _wallCheck.x *= -1;
    }
}