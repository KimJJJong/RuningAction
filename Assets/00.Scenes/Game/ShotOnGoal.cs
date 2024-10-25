using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ShotOnGoal : MonoBehaviour
{
    [SerializeField] GameObject[] goalObjects;
    [SerializeField] Transform[] balls;
    [SerializeField] Transform[] ballsPosition;

    public GoalKeeper goalKeeper;
    private Coroutine coroutine;
    private GameManager gameManager;

    private Coroutine eventCoroutine;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void ShootEvent(int playerLane)
    {
        goalKeeper.TryBlockShot(playerLane);
    }

    public void PlayGoalEvent()
    {
        PlayGoalText();

        //이벤트 실행
        int eventCount = System.Enum.GetValues(typeof(GoalEvent)).Length;
        GoalEvent goalEvent = (GoalEvent)Random.Range(0, eventCount);

        switch (goalEvent)
        {
            case GoalEvent.EventAddScore:
                gameManager.score.AddScore(1000);
                break;
            case GoalEvent.EventAddCoin:
                gameManager.score.AddCoin(300);
                break;
            case GoalEvent.EventAddHp:
                GameManager.Instance.HpController.Heal(10);
                break;
            default:
                Debug.LogError("Goal");
                break;
        }

        SetGoalObj();
    }

    public void PlayBlockEvent()
    {
        //이벤트 실행
        SetGoalObj();
    }

    public void SetGoalObj()
    {
        if (coroutine != null)
            coroutine = null;

        coroutine = StartCoroutine(SetGoalObjCor());
    }

    private void PlayGoalText()
    {
        if (eventCoroutine != null)
            eventCoroutine = null;

        eventCoroutine = StartCoroutine(GoalTextCor());
    }

    private IEnumerator GoalTextCor()
    {
        TextMeshProUGUI goalText = gameManager.collisions.goalText;
        goalText.enabled = true;
        goalText.GetComponent<DOTweenAnimation>().DOPlay();

        yield return new WaitForSeconds(2f);
        goalText.enabled = false;

        eventCoroutine = null;
    }

    private IEnumerator SetGoalObjCor()
    {
        yield return new WaitForSeconds(1f);

        foreach (GameObject obj in goalObjects)
        {
            obj.SetActive(false);
        }

        yield return new WaitForSeconds(2f);

        foreach (GameObject obj in goalObjects)
        {
            obj.SetActive(true);
        }

        ResetBallPosition();

        coroutine = null;
    }

    public void ResetBallPosition()
    {
        for(int num = 0; num < balls.Length; num++)
        {
            balls[num].position = ballsPosition[num].position;
        }
    }
}

public enum GoalEvent
{
    Null = 0,
    EventAddScore,
    EventAddCoin,
    EventAddHp
}
