using UnityEngine;

public class Player : LivingEntity
{
    protected override void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Animator anim = rb.GetComponent<Animator>();
        _movementSystem = new PlayerMovementSystem(_speed, _jumpForce, _groundLayer, transform, rb, anim);
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        _movementSystem.Update();
    }
}