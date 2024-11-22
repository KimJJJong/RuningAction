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

    [HideInInspector]
    public PlayController playController;

    [HideInInspector]
    public CollectCoin score;

    [Header("Character")]
    public GameObject player;
    public PlayerController playerController;
    public Collisions collisions;
    public StrengthenSubstance substance;
    public Weapon weapon;

    [Header("Camera Controller")]
    public PostEffectController postEffectController;
    public CameraFollowPlayer cameraFollowPlayer;

    private float currentPlayTime;

    public float CurrentPlayTime => currentPlayTime;

    void Awake()
    {
        _instance = GetComponent<GameManager>();

        //playerController = playController.GetCurrentController();
        //collisions = playerController.collisions;

        //player = playController.GetCurrentPlayer();
        //substance = player.GetComponent<StrengthenSubstance>();
        //weapon = player.GetComponent<Weapon>();
    }

    private void Start()
    {
        gameUiManager.SetGamePlayPanel();

        PlayController.OnPass.AddListener(
            (from, to) =>
            {
                SetPlayer(to);
            }
        );
    }

    private void Update()
    {
        if (gameState == GameState.Playing)
            currentPlayTime += Time.deltaTime;
    }

    public void SetPlayer(PlayerController pc)
    {
        playerController = pc;
        collisions = pc.collisions;

        player = playController.GetCurrentPlayer();
        substance = player.GetComponent<StrengthenSubstance>();
        weapon = player.GetComponent<Weapon>();

        score = playerController.GetComponent<CollectCoin>();
    }

    public void GamePlay()
    {
        gameState = GameState.Playing;
        OnGameStateChange.Invoke(gameState);

        cameraFollowPlayer.StartCameraMove(playController.GetCurrentPlayer().transform);
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
