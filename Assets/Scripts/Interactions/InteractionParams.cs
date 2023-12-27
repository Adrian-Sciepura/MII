public enum InteractionPriority
{
    Regular = 0,
    Quest = 10,
    Cutscene = 20,
    CRITICALLY_IMPORTANT = 40
}

[System.Serializable]
public class InteractionParams
{
    public InteractionPriority Priority = InteractionPriority.Regular;
    public bool freezeGameTime;
}