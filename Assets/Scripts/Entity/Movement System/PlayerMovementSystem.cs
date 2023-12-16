using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSystem : IMovementSystem
{
    private PlayerInputController _input;
    
    private Transform _transform;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private LayerMask _groundLayer;

    private int _moveSpeed;
    private int _jumpForce;

    private bool _isFacingRight;

    public PlayerMovementSystem(int moveSpeed, int jumpForce, LayerMask groundLayer, Transform transform, Rigidbody2D rigidBody, Animator animator)
    {
        _transform = transform;
        _rigidBody = rigidBody;
        _animator = animator;
        _groundLayer = groundLayer;

        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;

        _isFacingRight = true;

        _input = new PlayerInputController();
        _input.Enable();

        _input.Player.Jump.performed += Jump;
    }

    ~PlayerMovementSystem()
    {
        _input.Disable();
    }

    public void Update()
    {
        float horizontal = _input.Player.Move.ReadValue<Vector2>().x;

        _rigidBody.velocity = new Vector2(horizontal * _moveSpeed, _rigidBody.velocity.y);

        if ((!_isFacingRight && horizontal > 0f) || (_isFacingRight && horizontal < 0f))
            Flip();

        _animator.SetBool("isRunning", horizontal != 0);
        _animator.SetBool("isGrounded", IsGrounded());
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(IsGrounded())
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
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
        return Physics2D.Raycast(_transform.position, Vector2.down, 1.45f, _groundLayer);
    }
}