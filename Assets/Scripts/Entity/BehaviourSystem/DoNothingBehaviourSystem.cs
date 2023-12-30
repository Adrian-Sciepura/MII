using UnityEngine;

[System.Serializable]
public class DoNothingBehaviourSystem : IBehaviourSystem
{
    public GameEntity context { get; set; }

    public void Dispose()
    {

    }
}