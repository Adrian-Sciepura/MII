using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private int points;

    [SerializeField]
    private bool isKey;

    public void Collect()
    {
        GameManager.AddPoints(points);

        if (isKey)
            GameManager.AddKey();

        gameObject.SetActive(false);
        Destroy(this);
    }
}