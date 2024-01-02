public interface IMovementSystem
{
    GameEntity context { get; set; }
    void Update() { }
    void Dispose();
}