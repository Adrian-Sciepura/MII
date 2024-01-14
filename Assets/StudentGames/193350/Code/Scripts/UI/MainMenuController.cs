using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _newGameButton;

    [SerializeField]
    private Button _quitButton;

    private void Awake()
    {
        EventManager.DestroySingleton();
        GameManager.DestroySingleton();
        InteractionManager.DestroySingleton();
        LevelManager.DestroySingleton();
    }

    private void Start()
    {
        _newGameButton.onClick.AddListener(NewGame);
        _quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        _newGameButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("193350");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
