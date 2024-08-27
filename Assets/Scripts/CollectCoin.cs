using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectCoin : MonoBehaviour
{
    int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI EndScoreText;

    private int _increaseRate = 1;
    private int _increaseCoin = 30;
    private int _increaseObs= 50;

    int highScore;
    Color defaultScoreColor;

    public TextMeshProUGUI highScoreText;
    public Color highScoreColor; 
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        score = 0;
        scoreText.text = score.ToString();
        highScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = highScore.ToString();
        defaultScoreColor = scoreText.color; 

        StartCoroutine(UpdateScore());
    }

    public void setIncreasRate(int increasRate) // 시간마다 증가율
    {
        _increaseRate += increasRate;
    }
    public void setIncreasCoinRate(int increaseCoin)
    {
        _increaseCoin += increaseCoin;
    }
    public void setIncreasObsRate(int increasObs)
    {
        _increaseObs += increasObs;
    }
    public void increasObsScore()
    {
        score += _increaseObs;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            AddCoin();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Rush"))
        {
            GameManager.Instance.playerController.Invincibility(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Magnetic"))
        {
            Debug.Log("Magnetic");
            GameManager.Instance.playerController.BeMagnetic();
            Destroy(other.gameObject);
        }
    }
    public void AddCoin()
    {
        score += _increaseCoin;
        scoreText.text = score.ToString();
        CheckHighScoreColor();
    }
    IEnumerator UpdateScore()
    {
        while (true)
        {
            if (score <= highScore)
            {
                score += _increaseRate;
                scoreText.text = score.ToString();
                EndScoreText.text = score.ToString();
            }
            else if (score > highScore)
            {
                score += _increaseRate;
                EndScoreText.text = score.ToString();
                CheckHighScoreColor();
                highScore = score;
                highScoreText.text = highScore.ToString();
                PlayerPrefs.SetInt("highscore", highScore);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    void CheckHighScoreColor()
    {
        if (score > highScore)
        {
            scoreText.color = highScoreColor; 
        }
        else
        {
            scoreText.color = defaultScoreColor; 
        }
    }
}
