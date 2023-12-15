using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    Button newGameButton;

    [SerializeField]
    Button continueGameButton;

    [SerializeField]
    Button settingsButton;

    [SerializeField]
    Button quitButton;

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
