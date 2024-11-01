using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

    public GameState gameState = GameState.NotPlaying;

    public GameUIManager gameUiManager;

    [Header("Play Controller")]
    public PlayController playController;
    public CollectCoin score;
    public HpController hpController;

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

        playerController = playController.GetCurrentController();
        collisions = playerController.collisions;

        player = playController.GetCurrentPlayer();
        substance = player.GetComponent<StrengthenSubstance>();
        weapon = player.GetComponent<Weapon>();
    }

    private void Start()
    {
        gameUiManager.SetGamePlayPanel();
    }

    private void Update()
    {
        if(gameState == GameState.Playing)
            currentPlayTime += Time.deltaTime;
    }

    public void SetPlayer(PlayerController pc)
    {
        playerController = pc;
        collisions = pc.collisions;

        player = playController.GetCurrentPlayer();
        substance = player.GetComponent<StrengthenSubstance>();
        weapon = player.GetComponent<Weapon>();
    }

    public void GamePlay()
    {
        gameState = GameState.Playing;
        playController.SetRunningAnimation(true);
        hpController.StartHpControll();
        score.StartUpdateScore();

        cameraFollowPlayer.StartCameraMove(playController.GetCurrentPlayer().transform);
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        playerController.SetRunningAnimation(false);
        gameOver = true;
        gameUiManager.SetGameOverPanel();
        postEffectController.StopAllCoroutines();
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        gameState = GameState.NotPlaying;
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
    GameOver
}
