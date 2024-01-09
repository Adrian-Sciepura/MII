[System.Serializable]
public class CheckKeysCondition : IInteractionCondition
{
    public int keys;

    public bool CheckCondition()
    {
        return keys == GameManager.Keys;
    }
}