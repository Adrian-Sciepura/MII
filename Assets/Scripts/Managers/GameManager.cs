using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PAUSE,
    PLAYING,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private GameState _gameState;
    private float _gameTime;
    private int _points;
    private int _keys;
    private int _deathCount;
    private int _killCount;

    public static GameManager Instance => _instance;
    public static GameState GameState => _instance._gameState;
    public static float GameTime => _instance._gameTime;
    public static int Points => _instance._points;
    public static int Keys => _instance._keys;
    public static int DeathCount => _instance._deathCount;
    public static int KillCount => _instance._killCount;

    public static void AddKey()
    {
        if (_instance._keys == 3)
            return;

        _instance._keys++;
        EventManager.Publish(new OnKeysValueChanged());
    }
    public static void AddPoints(int ammount)
    {
        _instance._points += ammount;
        EventManager.Publish(new OnPointsValueChanged());
    }
    public static void IncreaseDeathCount() => _instance._deathCount++;
    public static void IncreaseKillCount() => _instance._killCount++;

    public static void DestroySingleton()
    {
        Destroy(_instance.gameObject);
        _instance = null;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _gameState = GameState.PLAYING;

        GameDataManager.InitGameData();
        GameDataManager.input.Player.Enable();
    }

    private void Update()
    {
        _gameTime += Time.deltaTime;
    }
}