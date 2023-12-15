using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 8f;

    [SerializeField]
    float _jumpForce = 10f;

    [SerializeField]
    Transform _groundCheck;

    [SerializeField]
    LayerMask _groundLayer;

    private bool _isFacingRight = true;
    private float _horizontal = 0f;
    private float _rayLength = 1.45f;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _rigidBody.velocity = new Vector2(_horizontal * _moveSpeed, _rigidBody.velocity.y);

        if ((!_isFacingRight && _horizontal > 0f) || (_isFacingRight && _horizontal < 0f))
            Flip();

        _animator.SetBool("isRunning", _horizontal != 0);
        _animator.SetBool("isGrounded", IsGrounded());
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);

        if (context.canceled && _rigidBody.velocity.y > 0f)
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * 0.5f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
