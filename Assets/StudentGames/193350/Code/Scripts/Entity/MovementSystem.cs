using System.Collections;
using UnityEngine;

public abstract class MovementSystem : MonoBehaviour
{
    protected Rigidbody2D _rigidBody;
    protected bool _isFacingRight;
    protected bool _isKnockbackActive;

    protected virtual void Awake()
    {
        _isFacingRight = true;
        _rigidBody = GetComponent<Rigidbody2D>();
        _isKnockbackActive = false;
    }
    protected virtual void Update()
    { }
    protected virtual void Start()
    { }
    protected virtual void OnDestroy()
    { }
    protected virtual void OnEnable()
    { }
    protected virtual void OnDisable()
    { }
    protected virtual void OnDrawGizmos()
    { }

    protected void SetVelocity(float x, float y)
    {
        if (!_isKnockbackActive)
            _rigidBody.velocity = new Vector2(x, y);
    }

    protected void ResetKnockback()
    {
        _isKnockbackActive = false;
    }

    public void AddKnockback(Vector2 force)
    {
        _isKnockbackActive = true;
        _rigidBody.velocity = force;
        Invoke("ResetKnockback", 0.5f);
    }
}