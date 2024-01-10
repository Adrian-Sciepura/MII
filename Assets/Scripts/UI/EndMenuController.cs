using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _pointsText;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    [SerializeField]
    private TextMeshProUGUI _killsAndDeathsText;

    [SerializeField]
    private Button _mainMenuButton;

    [SerializeField]
    private Button _quitButton;


    private void Awake()
    {
        int time = (int)GameManager.GameTime;
        _pointsText.text = $"Zebrane punkty ECTS: {GameManager.Points.ToString("0000000")}";
        _timeText.text = $"Czas ukonczenia gry: {string.Format("{0:00}:{1:00}", time / 60, time % 60)}";
        _killsAndDeathsText.text = $"Przeciwnicy: {GameManager.KillCount}               Smierci: {GameManager.DeathCount}";

        EventManager.DestroySingleton();
        GameManager.DestroySingleton();
        InteractionManager.DestroySingleton();
        LevelManager.DestroySingleton();

        _mainMenuButton.onClick.AddListener(MainMenu);
        _quitButton.onClick.AddListener(Quit);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Quit()
    {
        Application.Quit();
    }
}