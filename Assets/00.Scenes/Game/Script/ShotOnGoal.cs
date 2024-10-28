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

    public void ShootEvent(int playerLane)
    {
        goalKeeper.TryBlockShot(playerLane);
    }

    public void PlayGoalEvent()
    {
        GameUIManager.instance.PlayGoalText();

        //이벤트 실행
        int eventCount = System.Enum.GetValues(typeof(GoalEvent)).Length;
        GoalEvent goalEvent = (GoalEvent)Random.Range(0, eventCount);

        switch (goalEvent)
        {
            case GoalEvent.EventAddScore:
                //gameManager.score.AddScore(1000);
                Debug.LogError("Score Event");
                break;
            case GoalEvent.EventAddCoin:
                //gameManager.score.AddCoin(300);
                Debug.LogError("Coin Event");
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
        {
            StopCoroutine(coroutine); 
        }

        coroutine = StartCoroutine(SetGoalObjCor());
    }

    private IEnumerator SetGoalObjCor()
    {
        yield return new WaitForSeconds(0.3f);

        foreach (GameObject obj in goalObjects)
        {
            obj.SetActive(false);
        }
        ResetPosition();

        yield return new WaitForSeconds(2f);

        foreach (GameObject obj in goalObjects)
        {
            obj.SetActive(true);
        }

        coroutine = null;
    }

    public void ResetPosition()
    {
        for(int num = 0; num < balls.Length; num++)
        {
            balls[num].position = ballsPosition[num].position;
            balls[num].rotation = Quaternion.identity;
        }

        goalKeeper.SetKeeperLane();
    }
}

public enum GoalEvent
{
    Null = 0,
    EventAddScore,
    EventAddCoin,
    EventAddHp
}
