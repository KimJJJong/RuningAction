using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotOnGoal : MonoBehaviour
{
    [SerializeField] GameObject[] goalObjects;
    [SerializeField] Transform[] balls;
    [SerializeField] Transform[] ballsPosition;

    public GoalKeeper goalKeeper;

    private void OnEnable()
    {
        SetGoalKeeperAndGoalPost(true);
    }

    public void ShootEvent(int playerLane)
    {
        goalKeeper.TryBlockShot(playerLane);
    }

    public void GoalEvent()
    {
        //이벤트 실행
        SetGoalKeeperAndGoalPost(false);
        ResetBallPosition();
    }

    public void SetGoalKeeperAndGoalPost(bool isActive)
    {
        foreach (GameObject obj in goalObjects)
        {
            if (obj.activeSelf != isActive)
                obj.SetActive(isActive);
        }
    }

    public void ResetBallPosition()
    {
        for(int num = 0; num < balls.Length; num++)
        {
            balls[num].position = ballsPosition[num].position;
        }
    }
}
