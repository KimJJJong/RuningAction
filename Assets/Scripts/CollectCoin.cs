using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectCoin : MonoBehaviour
{
    int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI EndScoreText;
    int increaseRate = 1;
    int highScore;
    public TextMeshProUGUI highScoreText;
    public Color highScoreColor; 
    Color defaultScoreColor;
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
        highScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = highScore.ToString();
        defaultScoreColor = scoreText.color; 
    }
    void FixedUpdate()
    {
        UpdateScore();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            AddCoin();
            Destroy(other.gameObject);
        }
    }
    public void AddCoin()
    {
        score += 50;
        scoreText.text = score.ToString();
        CheckHighScoreColor();
    }
    public void UpdateScore()
    {
        if (score <= highScore)
        {
            score += increaseRate;
            scoreText.text = score.ToString();
            EndScoreText.text =score.ToString();
        }
        else if (score > highScore)
        {
            score += increaseRate;
            EndScoreText.text = score.ToString();
            CheckHighScoreColor();
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("highscore", highScore);
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
