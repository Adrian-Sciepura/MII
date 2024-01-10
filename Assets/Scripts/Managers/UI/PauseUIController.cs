using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUIController : MonoBehaviour
{
    [SerializeField]
    private Button _menuButton;

    [SerializeField]
    private Button _exitButton;

    private void Awake()
    {
        _menuButton.onClick.AddListener(Menu);
        _exitButton.onClick.AddListener(Quit);
        GameDataManager.input.Player.Pause.performed += OnPause;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
        GameDataManager.input.Player.Pause.performed -= OnPause;
    }

    private void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        gameObject.SetActive(!gameObject.activeSelf);
    }
}