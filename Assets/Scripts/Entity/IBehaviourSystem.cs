using UnityEngine;

public interface IBehaviourSystem
{
    void Update();
    void OnInteractionAreaEnter(Collider2D other);
    void OnInteractionAreaExit(Collider2D other);
    void SetContext(GameEntity context);
    void Dispose();
}