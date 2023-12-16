using UnityEngine;

public enum GameEntityType
{
    Player,
    Enemy,
    DroppedItem,
    Block
}

public abstract class GameEntity : MonoBehaviour
{
    [SerializeField]
    protected LayerMask _groundLayer;

    protected GameEntityType _entityType;


    protected abstract void Awake();

    protected abstract void Start();

    protected abstract void Update();

    protected bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.2f, _groundLayer);
    }
}
