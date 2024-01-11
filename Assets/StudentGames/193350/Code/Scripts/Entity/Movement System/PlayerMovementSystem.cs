using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerMovementSystem : WalkMovementSystemTemplate
{
    private PlayerInputController _input;

    protected override void Awake()
    {
        base.Awake();
        _input = GameDataManager.input;
        _input.Player.Jump.performed += Jump;
    }

    protected override void Update()
    {
        float horizontal = _input.Player.Move.ReadValue<Vector2>().x;

        SetVelocity(horizontal * _movementData.speed, _rigidBody.velocity.y);

        if ((!_isFacingRight && horizontal > 0f) || (_isFacingRight && horizontal < 0f))
            Flip();

        _animator.SetBool("isRunning", horizontal != 0);
        _animator.SetBool("isGrounded", IsGrounded());
    }

    protected override void OnDestroy()
    {
        _input.Player.Jump.performed -= Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
            SetVelocity(_rigidBody.velocity.x, _movementData.jumpForce);
    }
}