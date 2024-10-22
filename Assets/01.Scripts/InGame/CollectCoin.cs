using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DarkTonic.MasterAudio;

public class CollectCoin : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI EndScoreText;
    public TMP_Text coinText;
    public TextMeshProUGUI highScoreText;
    public Color highScoreColor;

    int score;
    int coin = 0;
    int highScore;

    private int _increaseRate = 1;
    private int _increaseCoin = 30;
    private int _increaseObs = 50;

    Color defaultScoreColor;

    private void Start()
    {
        InitializeScore();
        StartCoroutine(UpdateScore());
    }

    private void InitializeScore()
    {
        PlayerPrefs.DeleteAll();
        scoreText.text = $"Score : {score}";
        highScore = DataManager.instance.userData.userScore;
        highScoreText.text = highScore.ToString();
        defaultScoreColor = scoreText.color;
    }

    public void SetIncreasRate(int increasRate) 
    {
        _increaseRate += increasRate;
    }

    public void SetIncreasCoinRate(int increaseCoin)
    {
        _increaseCoin += increaseCoin;
    }

    public void SetIncreasObsRate(int increasObs)
    {
        _increaseObs += increasObs;
    }

    public void IncreasObsScore()
    {
        AddScore(_increaseObs);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Coin":
                AddCoin();
                MasterAudio.PlaySound("item_acquired");
                Destroy(other.gameObject);
                break;
            case "Rush":
                GameManager.Instance.playerController.Invincibility(true);
                Destroy(other.gameObject);
                break;
            case "Magnetic":
                GameManager.Instance.playerController.BeMagnetic();
                Destroy(other.gameObject);
                break;
            case "Heal":
                GameManager.Instance.HpController.Heal(10);
                Destroy(other.gameObject);
                break;
        }
    }

    public void AddCoin()
    {
        AddScore(_increaseCoin);
        coin += _increaseCoin;
        coinText.text = coin.ToString();
        CheckHighScoreColor();
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

    private IEnumerator UpdateScore()
    {
        while (true)
        {
            AddScore(_increaseRate);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddScore(int _score)
    {
        score += _score;
        scoreText.text = $"Score : {score}";
        EndScoreText.text = score.ToString();
        if (score > highScore)
        {
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        highScore = score;
        highScoreText.text = highScore.ToString();
        DataManager.instance.userData.userScore = highScore;
    }

}
