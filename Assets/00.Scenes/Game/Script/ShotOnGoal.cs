using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShotOnGoal : MonoBehaviour
{
    private GameUIManager gameUiManager;

    [SerializeField]
    GameObject[] goalObjects;

    [SerializeField]
    Transform[] balls;

    [SerializeField]
    Transform[] ballsPosition;

    public GoalKeeper goalKeeper;
    private Coroutine coroutine;

    private void Start()
    {
        gameUiManager = GameUIManager.instance;
    }

    public void ShootEvent(int playerLane)
    {
        goalKeeper.TryBlockShot(playerLane);
    }

    public void PlayGoalEvent()
    {
        GameUIManager.instance.PlayGoalText();
        PlayerController controller = GameManager.Instance.playerManager.GetCurrentController();
        //�̺�Ʈ ����
        int eventCount = System.Enum.GetValues(typeof(GoalEvent)).Length;
        GoalEvent goalEvent = (GoalEvent)Random.Range(0, eventCount);

        switch (goalEvent)
        {
            case GoalEvent.EventAddScore:
                gameUiManager.UpdateScoreText(2000);
                Debug.LogError("Score Event");
                break;
            case GoalEvent.EventAddCoin:
                gameUiManager.UpdateScoreText(2000);
                Debug.LogError("Coin Event");
                break;
            case GoalEvent.EventAddHp:
                gameUiManager.UpdateScoreText(2000);
                HpController.Heal(controller.playerObj, 10);
                //GameManager.Instance.hpController.Heal(10);
                break;
            default:
                gameUiManager.UpdateScoreText(2000);
                Debug.LogError("Goal");
                break;
        }

        SetGoalObj();
    }

    public void PlayBlockEvent()
    {
        //�̺�Ʈ ����
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
        for (int num = 0; num < balls.Length; num++)
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
    EventAddHp,
}
