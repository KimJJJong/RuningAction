using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;

public class AddScoreEvent : UnityEngine.Events.UnityEvent<int> { }

public class CollectCoin : MonoBehaviour
{
    private GameUIManager gameUiManager;

    private int score;
    private int coin = 0;
    private int highScore;

    private int _increaseRate = 1;
    private int _increaseCoin = 30;
    private int _increaseObs = 50;

    private void Start()
    {
        gameUiManager = GameUIManager.instance;

        InitializeScore();

        GameManager.OnGameStateChange.AddListener(
            (state) =>
            {
                if (state == GameState.Playing)
                {
                    StartUpdateScore();
                }
            }
        );
    }

    private void InitializeScore()
    {
        PlayerPrefs.DeleteAll();
        gameUiManager.UpdateScoreText(score);
        highScore = DataManager.instance.userData.userScore;
        gameUiManager.UpdateHighScoreText(highScore);
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
        PlayerController controller = GameManager.Instance.playerManager.GetCurrentController();

        switch (other.tag)
        {
            case "Coin":
                AddCoin();
                MasterAudio.PlaySound("item_acquired");
                Destroy(other.gameObject);
                break;
            case "Rush":
                GameManager.Instance.playerManager.GetCurrentController().Invincibility(true);
                Destroy(other.gameObject);
                break;
            case "Magnetic":
                GameManager.Instance.playerManager.GetCurrentController().BeMagnetic();
                Destroy(other.gameObject);
                break;
            case "Heal":
                HpController.Heal(controller.playerObj, 10);
                //GameManager.Instance.hpController.Heal(10);
                Destroy(other.gameObject);
                break;
        }
    }

    public void AddCoin()
    {
        AddScore(_increaseCoin);
        coin += _increaseCoin;
        gameUiManager.UpdateCoinText(coin);
        gameUiManager.UpdateEndCoinText(coin);
        gameUiManager.CheckHighScoreColor(score, highScore);
    }

    public void AddCoin(int coinValue)
    {
        AddScore(coinValue);
        coin += coinValue;
        gameUiManager.UpdateCoinText(coin);
        gameUiManager.UpdateEndCoinText(coin);
        gameUiManager.CheckHighScoreColor(score, highScore);
    }

    public void StartUpdateScore()
    {
        //StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        while (GameManager.Instance.gameState == GameState.Playing)
        {
            AddScore(_increaseRate);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddScore(int _score)
    {
        score += _score;

        gameUiManager.UpdateScoreText(score);
        gameUiManager.UpdateEndScoreText(score);
        if (score > highScore)
        {
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        highScore = score;
        gameUiManager.UpdateHighScoreText(highScore);
        DataManager.instance.userData.userScore = highScore;
    }
}
