using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerMovementSystem : IMovementSystem
{
    private GameEntity _context;

    private PlayerInputController _input;
    private Transform _transform;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private LandEntityData _movementData;

    private bool _isFacingRight;

    public void Update()
    {
        float horizontal = _input.Player.Move.ReadValue<Vector2>().x;

        _rigidBody.velocity = new Vector2(horizontal * _movementData.speed, _rigidBody.velocity.y);

        if ((!_isFacingRight && horizontal > 0f) || (_isFacingRight && horizontal < 0f))
            Flip();

        _animator.SetBool("isRunning", horizontal != 0);
        _animator.SetBool("isGrounded", IsGrounded());
    }

    public void SetContext(GameEntity entity)
    {
        _context = entity;

        _input = GameDataManager.input;
        _input.Player.Jump.performed += Jump;

        _isFacingRight = true;
        _transform = _context.transform;
        _rigidBody = _context.GetComponent<Rigidbody2D>();
        _animator = _context.GetComponent<Animator>();
        _movementData = _context.entityData.GetData<LandEntityData>();
    }

    public void Dispose()
    {
        _input.Player.Jump.performed -= Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _movementData.jumpForce);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = _transform.localScale;
        localScale.x *= -1;
        _transform.localScale = localScale;
    }

    protected bool IsGrounded()
    {
        return Physics2D.Raycast(_transform.position, Vector2.down, 1.45f, _movementData.groundLayer);
    }
}