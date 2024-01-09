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

    public static GameManager Instance => _instance;
    public static GameState GameState => _instance._gameState;
    public static float GameTime => _instance._gameTime;
    public static int Points => _instance._points;
    public static int Keys => _instance._keys;


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
        EventManager.Subscribe<OnInteractionItemStartEvent<ChangeSceneInteractionItem>>(ChangeScene);
    }

    private void Update()
    {
        _gameTime += Time.deltaTime;
    }

    private void ChangeScene(OnInteractionItemStartEvent<ChangeSceneInteractionItem> onSceneChange)
    {
        SceneManager.LoadScene(onSceneChange.Data.sceneName);
        EventManager.Publish(new OnInteractionItemFinishEvent<ChangeSceneInteractionItem>());
    }
}