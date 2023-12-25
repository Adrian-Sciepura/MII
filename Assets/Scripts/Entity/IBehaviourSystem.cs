public interface IBehaviourSystem
{
    void Update();
    void OnInteractionAreaEnter(GameEntity other);
    void OnInteractionAreaExit(GameEntity other);
    void SetContext(GameEntity context);
    void Dispose();
}