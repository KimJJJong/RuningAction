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

    [HideInInspector] public GameUIManager gameUiManager;
    [HideInInspector] public GameObject player;
    [HideInInspector] public CollectCoin score;
    [HideInInspector] public Collisions collisions;
    [HideInInspector] public StrengthenSubstance substance;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public HpController hpController;
    [HideInInspector] public Weapon weapon;
    [HideInInspector] public PostEffectController postEffectController;
    [HideInInspector] public CameraFollowPlayer cameraFollowPlayer;

    private float currentPlayTime;

    public float CurrentPlayTime => currentPlayTime;
    void Awake()
    {
        _instance = GetComponent<GameManager>();
        gameUiManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        score = GameObject.Find("Player").GetComponent<CollectCoin>();
        collisions = GameObject.Find("Player").GetComponent<Collisions>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        substance = GameObject.Find("Player").GetComponent<StrengthenSubstance>();
        hpController = GameObject.Find("Player").GetComponent<HpController>();
        weapon = GameObject.Find("Player").GetComponent<Weapon>();
        postEffectController = GameObject.Find("Main Camera").GetComponent<PostEffectController>();

        cameraFollowPlayer = GameObject.Find("Main Camera").AddComponent<CameraFollowPlayer>();
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

    public void GamePlay()
    {
        gameState = GameState.Playing;
        playerController.SetRunningAnimation(true);
        hpController.StartHpControll();
        score.StartUpdateScore();
        cameraFollowPlayer.StartCameraMove();
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        playerController.SetRunningAnimation(false);
        gameOver = true;
        gameUiManager.SetGameOverPanel();
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
