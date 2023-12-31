public interface IMovementSystem
{
    void Update();
    void SetContext(GameEntity entity);
    void Dispose();
}