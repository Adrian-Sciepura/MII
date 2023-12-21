using UnityEngine;

public enum GameEntityType
{
    Player,
    Enemy,
    Boss,
    DroppedItem,
    Block
}

public class GameEntity : MonoBehaviour
{
    public EntityDataContainer entityData;
    private IBehaviourSystem _behaviourSystem;
    private IMovementSystem _movementSystem;

    private void Awake()
    {
        entityData = new EntityDataContainer();
    }

    private void Update()
    {
        _behaviourSystem?.Update();
        _movementSystem?.Update();
    }

    public void SetBehaviourSystem(IBehaviourSystem newBehaviourSystem)
    {
        _behaviourSystem = newBehaviourSystem;
        _behaviourSystem.SetContext(this);
    }

    public void SetMovementSystem(IMovementSystem newMovementSystem)
    {
        if(_movementSystem != null)
            _movementSystem.Dispose();

        _movementSystem = newMovementSystem;
        _movementSystem.SetContext(this);
    }
}
