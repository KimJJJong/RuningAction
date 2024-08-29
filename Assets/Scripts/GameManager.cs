using UnityEngine;
using UnityEditor;
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
    public GameObject gameOverPanel;
    public GameObject player;
    public CollectCoin score;
    public Collisions collisions;
    public StrengthenSubstance substance;
    public PlayerController playerController;
    public HpController HpController;
    void Awake()
    {
        _instance = GetComponent<GameManager>();
        score = GameObject.Find("Player").GetComponent<CollectCoin>();
        collisions =GameObject.Find("Player").GetComponent<Collisions>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        substance = GameObject.Find("Player").GetComponent<StrengthenSubstance>();
        HpController =GameObject.Find("Player").GetComponent<HpController>();
    }


    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        //EditorApplication.isPlaying = false;
    }

}
