using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DarkTonic.MasterAudio;

public class CollectCoin : MonoBehaviour
{
    int score;
    int coin = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI EndScoreText;
    public TMP_Text coinText;

    private int _increaseRate = 1;
    private int _increaseCoin = 30;
    private int _increaseObs = 50;

    int highScore;
    Color defaultScoreColor;

    public TextMeshProUGUI highScoreText;
    public Color highScoreColor;
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        score = 0;
        scoreText.text = "Score : " + score.ToString();
        highScore = PlayerPrefs.GetInt("highscore");
        highScoreText.text = highScore.ToString();
        defaultScoreColor = scoreText.color;

        StartCoroutine(UpdateScore());
    }

    public void SetIncreasRate(int increasRate) // �ð����� ������
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
        score += _increaseObs;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            AddCoin();
            MasterAudio.PlaySound("GetItem");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Rush"))
        {
            //Debug.Log("Rush");
            GameManager.Instance.playerController.Invincibility(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Magnetic"))
        {
            //Debug.Log("Magnetic");
            GameManager.Instance.playerController.BeMagnetic();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Heal"))
        {
            //Debug.Log("Heal");
            GameManager.Instance.HpController.Heal(10);
            Destroy(other.gameObject);

        }
    }
    public void AddCoin()
    {
        score += _increaseCoin;
        coin += _increaseCoin;
        scoreText.text = "Score : " + score.ToString();
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

    public void AddScore(int _score)
    {
        score += _score;

    }

    IEnumerator UpdateScore()
    {
        while (true)
        {
            if (score <= highScore)
            {
                score += _increaseRate;
                scoreText.text = "Score : " + score.ToString();
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

}
