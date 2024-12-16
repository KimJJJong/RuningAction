using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameStateChangeEvent : UnityEngine.Events.UnityEvent<GameState> { }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject container = new GameObject("GameManager");
                _instance = container.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    public static bool gameOver = false;

    public static GameStateChangeEvent OnGameStateChange = new GameStateChangeEvent();
    public GameState gameState = GameState.NotPlaying;

    public GameUIManager gameUiManager;

    public PlayerManager playerManager;

    public MapManager mapManager;

    [Header("Camera Controller")]
    public PostEffectController postEffectController;
    public CameraManager camera_manager;

    private float currentPlayTime;

    public float CurrentPlayTime => currentPlayTime;

    public enum Lane
    {
        NotDecidedYet = -1,
        Left,
        Center,
        Right,
    }

    float mapCenter = 0;
    float laneGap = 2;
    public Dictionary<Lane, float> lanePositions = new Dictionary<Lane, float>();

    #region MapManager

    [SerializeField]
    private float map_speed = 5.0f;

    [SerializeField]
    private float max_game_speed = 5.0f;

    [HideInInspector]
    public float maxGameSpeed
    {
        get { return max_game_speed; }
    }

    [SerializeField]
    private float game_speed = 1f;

    [HideInInspector]
    public float gameSpeed
    {
        get { return game_speed; }
    }

    public float GetInitialMapSpeed()
    {
        return map_speed;
    }

    #endregion


    void Awake()
    {
        OnGameStateChange = new GameStateChangeEvent();
        _instance = GetComponent<GameManager>();

        lanePositions[Lane.Left] = mapCenter - laneGap;
        lanePositions[Lane.Center] = mapCenter;
        lanePositions[Lane.Right] = mapCenter + laneGap;
    }

    private void Start()
    {
        gameUiManager.SetGamePlayPanel();
    }

    private void Update()
    {
        if (gameState == GameState.Playing)
        {
            currentPlayTime += Time.deltaTime;
            if (game_speed < maxGameSpeed)
                game_speed += Time.deltaTime * 0.01f;
        }
    }

    public void GamePlay()
    {
        MasterAudio.PlaySound("game_start");
        gameState = GameState.Playing;
        OnGameStateChange.Invoke(gameState);

        camera_manager.CameraSetting();
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        OnGameStateChange.Invoke(gameState);

        gameOver = true;
        gameUiManager.SetGameOverPanel();
        postEffectController.StopAllCoroutines();
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        gameState = GameState.NotPlaying;
        OnGameStateChange.Invoke(gameState);

        gameOver = false;
        gameUiManager.SetGamePlayPanel();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        //EditorApplication.isPlaying = false;
    }

    public void LoadScene(int num)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(num);
    }
}

public enum GameState
{
    NotPlaying = 0,
    Playing,
    GameOver,
}
