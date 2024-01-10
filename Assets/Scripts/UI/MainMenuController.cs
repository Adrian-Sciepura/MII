using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(Quit);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("audytorium");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
