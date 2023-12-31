using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueGameButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(NewGame);
        continueGameButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(Settings);
        quitButton.onClick.AddListener(Quit);
    }

    public void NewGame()
    {

    }


    public void ContinueGame()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {

    }
}
