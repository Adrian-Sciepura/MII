using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public sealed class PlayerMovementSystem : WalkMovementSystemTemplate
{
    private PlayerInputController _input;

    public override GameEntity context
    {
        get => _context;
        set
        {
            base.context = value;

            _input = GameDataManager.input;
            _input.Player.Jump.performed += Jump;
        }
    }

    public override void Update()
    {
        float horizontal = _input.Player.Move.ReadValue<Vector2>().x;

        _rigidBody.velocity = new Vector2(horizontal * _movementData.speed, _rigidBody.velocity.y);

        if ((!_isFacingRight && horizontal > 0f) || (_isFacingRight && horizontal < 0f))
            Flip();

        _animator.SetBool("isRunning", horizontal != 0);
        _animator.SetBool("isGrounded", IsGrounded());
    }

    public override void Dispose()
    {
        _input.Player.Jump.performed -= Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _movementData.jumpForce);
    }
}