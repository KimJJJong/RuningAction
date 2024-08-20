using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject[] player;
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

    void Awake()
    {
        _instance = GetComponent<GameManager>();
        if (!GameObject.Find("Player"))
        {
            GameObject clo = Instantiate(player[0], GameObject.Find("Center").transform.position, Quaternion.identity);
            clo.name = "Player";
        }
    }

    void Update()
    {
        
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
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
