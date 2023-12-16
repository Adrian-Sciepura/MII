using UnityEngine;

public abstract class LivingEntity : GameEntity
{
    [SerializeField]
    protected int _speed;

    [SerializeField]
    protected int _jumpForce;

    [SerializeField]
    protected int _maxHealth;

    protected int _health;

    protected IMovementSystem _movementSystem;
}