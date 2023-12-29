using UnityEngine;

public interface IBehaviourSystem
{
    void Update();
    void OnTriggerEnter(Collider2D other);
    void OnTriggerLeave(Collider2D other);
    void SetContext(GameEntity context);
    void Dispose();
}