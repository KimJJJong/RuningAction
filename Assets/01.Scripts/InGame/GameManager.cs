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
    public GameObject gameOverPanel;
    public GameObject hpBar;
    public GameObject avilBar;

    [HideInInspector] public GameObject player;
    [HideInInspector] public CollectCoin score;
    [HideInInspector] public Collisions collisions;
    [HideInInspector] public StrengthenSubstance substance;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public HpController HpController;
    [HideInInspector] public Weapon weapon;
    [HideInInspector] public PostEffectController postEffectController;

    void Awake()
    {
        _instance = GetComponent<GameManager>();
        score = GameObject.Find("Player").GetComponent<CollectCoin>();
        collisions = GameObject.Find("Player").GetComponent<Collisions>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        substance = GameObject.Find("Player").GetComponent<StrengthenSubstance>();
        HpController = GameObject.Find("Player").GetComponent<HpController>();
        weapon = GameObject.Find("Player").GetComponent<Weapon>();
        postEffectController = GameObject.Find("Main Camera").GetComponent<PostEffectController>();
    }


    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        hpBar.SetActive(false);
        avilBar.SetActive(false);
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        gameOver = false;
        gameOverPanel.SetActive(false);
        hpBar.SetActive(true);
        avilBar.SetActive(true);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
