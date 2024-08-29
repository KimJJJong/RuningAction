using UnityEngine;

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

    void Awake()
    {
        //  _instance = GetComponent<GameManager>();
        score = GameObject.Find("Player").GetComponent<CollectCoin>();
        player = GameObject.Find("Player");
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
        //UnityEditor.EditorApplication.isPlaying = false;
    }

}
