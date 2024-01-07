public enum InteractionPriority
{
    Regular = 0,
    Quest = 10,
    Cutscene = 20,
    CRITICALLY_IMPORTANT = 999
}

[System.Serializable]
public class InteractionParams
{
    public InteractionPriority Priority = InteractionPriority.Regular;
    public bool freezeGameTime;
    public bool turnOffPlayerInput;
    public bool destroyAfterPlay;
}