using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void GoalEvent()
    {
        //이벤트 실행
        SetGoalObj();
    }

    public void BlockEvent()
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
