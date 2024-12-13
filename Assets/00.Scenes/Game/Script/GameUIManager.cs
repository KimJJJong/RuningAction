using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [SerializeField]
    private GameManager gameManager;

    [Header("InGame UI")]
    [SerializeField]
    private GameObject gameUiPanel;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private TextMeshProUGUI goalText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI coinText;

    [Header("GameOver UI")]
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI runningTimeText;

    [SerializeField]
    private TextMeshProUGUI endCoinText;

    [SerializeField]
    private TextMeshProUGUI endScoreText;

    [SerializeField]
    private Color highScoreColor;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private Slider expSlider;

    [SerializeField]
    private float expFillSpeedMin = 200f;

    [SerializeField]
    private float expFillSpeedMax = 1000f;
    private Color defaultScoreColor;

    private WaitForSecondsRealtime textEffectTime = new WaitForSecondsRealtime(0.5f);

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        defaultScoreColor = scoreText.color;
    }

    public void SetGamePlayPanel()
    {
        gameOverPanel.SetActive(false);
        gameUiPanel.SetActive(true);

        //StartCountdown();
    }

    public void StartCountdown()
    {
        // Sequence countdownSequence = DOTween.Sequence();

        // countdownText.transform.localScale = Vector3.zero;
        // countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, 1);

        // countdownSequence.AppendCallback(() => UpdateCountdownText("3"))
        //                  .Append(countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        //                  .Append(countdownText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack))

        //                  .AppendCallback(() => UpdateCountdownText("2"))
        //                  .Append(countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        //                  .Append(countdownText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack))

        //                  .AppendCallback(() => UpdateCountdownText("1"))
        //                  .Append(countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        //                  .Append(countdownText.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack))

        //                  .AppendCallback(() => UpdateCountdownText("Start!"))
        //                  .Append(countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        //                  .Append(countdownText.DOFade(0, 0.5f))
        //                  .OnComplete(() =>
        //                  {
        //                      countdownText.gameObject.SetActive(false);
        //                      GameManager.Instance.GamePlay();
        //                  });
    }

    private void UpdateCountdownText(string text)
    {
        countdownText.text = text;
        countdownText.transform.localScale = Vector3.zero;
    }

    public void CheckHighScoreColor(int score, int highScore)
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

    public void PlayGoalText()
    {
        StartCoroutine(GoalTextCor());
    }

    private IEnumerator GoalTextCor()
    {
        goalText.enabled = true;
        goalText.GetComponent<DOTweenAnimation>().DOPlay();

        yield return new WaitForSeconds(2f);
        goalText.enabled = false;
    }

    public void SetGameOverPanel()
    {
        UpdateRunningTimeText(gameManager.CurrentPlayTime);

        gameOverPanel.SetActive(true);
        gameUiPanel.SetActive(false);

        StartCoroutine(ShowGameOverText());
    }

    private IEnumerator ShowGameOverText()
    {
        UpdateExpText(600, 1000);

        runningTimeText.enabled = true;
        ShowTextWithEffect(runningTimeText);
        yield return textEffectTime;

        endCoinText.enabled = true;
        ShowTextWithEffect(endCoinText);
        yield return textEffectTime;

        endScoreText.enabled = true;
        ShowTextWithEffect(endScoreText);
        yield return textEffectTime;

        highScoreText.enabled = true;
        ShowTextWithEffect(highScoreText);
        yield return textEffectTime;

        UpdateExpSlide(1000);
    }

    public void ShowTextWithEffect(TextMeshProUGUI textTMP)
    {
        textTMP.transform.localScale = Vector3.zero;

        if (string.IsNullOrEmpty(textTMP.text))
            textTMP.text = "0";

        Sequence scoreSequence = DOTween.Sequence().SetUpdate(true);
        scoreSequence
            .Append(textTMP.transform.DOScale(Vector3.one * 1.5f, 0.2f))
            .Append(textTMP.transform.DOScale(Vector3.one, 0.1f))
            .Append(textTMP.transform.DOShakeScale(0.3f, 0.2f, 10, 90f));
    }

    public void UpdateExpSlide(int gainedExp)
    {
        StartCoroutine(FillExpSlider(gainedExp));
    }

    private IEnumerator FillExpSlider(int exp)
    {
        while (exp > 0)
        {
            float expToFill = Mathf.Min(expSlider.maxValue - expSlider.value, exp);
            float fillSpeed = Mathf.Lerp(
                expFillSpeedMin,
                expFillSpeedMax,
                exp / expSlider.maxValue
            );

            while (expToFill > 0)
            {
                expSlider.value += fillSpeed * Time.unscaledDeltaTime;
                expToFill -= fillSpeed * Time.unscaledDeltaTime;
                exp -= (int)(fillSpeed * Time.unscaledDeltaTime);

                if (expSlider.value >= expSlider.maxValue)
                {
                    int level = int.Parse(levelText.text);
                    level++;
                    levelText.text = level.ToString();
                    expSlider.value = 0;

                    expToFill = Mathf.Min(expSlider.maxValue, exp);
                }

                yield return null;
            }
        }
    }

    #region Text Update
    public void UpdateScoreText(int score)
    {
        scoreText.text = $"{score}";
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text = $"{coins}";
    }

    public void UpdateRunningTimeText(float time)
    {
        float roundedTime = Mathf.Round(time * 10f) / 10f;
        runningTimeText.text = $"{roundedTime}s";
    }

    public void UpdateEndCoinText(int gold)
    {
        endCoinText.text = $"{gold}";
    }

    public void UpdateEndScoreText(int endScore)
    {
        endScoreText.text = $"{endScore}";
    }

    public void UpdateHighScoreText(int highScore)
    {
        highScoreText.text = $"{highScore}";
    }

    public void UpdateExpText(int maxExp, int gainedExp)
    {
        expSlider.value = 0;
        expSlider.maxValue = maxExp;

        expText.text = $"+{gainedExp}";
    }
    #endregion
}
