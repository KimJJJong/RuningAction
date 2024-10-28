using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform rightPos;

    [SerializeField] private GameObject dangerIcon1;
    [SerializeField] private GameObject dangerIcon2;

    public int currentLane;
    public int adjacentLane;
    private ShotOnGoal shotOnGoal;

    private void Start()
    {
        shotOnGoal = transform.parent.GetComponent<ShotOnGoal>();
        SetKeeperLane();
    }


    public void SetKeeperLane()
    {
        currentLane = Random.Range(0, 3);
        transform.position = GetLanePosition(currentLane);

        SetAdjacentLane();

        ShowDangerIcon();
    }


    void SetAdjacentLane()
    {
        if (currentLane == 0 || (currentLane == 2))
        {
            adjacentLane = 1;
        }
        else
        {
            adjacentLane = Random.Range(0, 2) == 0 ? 0 : 2; 
        }
    } 

    public void TryBlockShot(int playerLane)
    {
        if (playerLane == currentLane || playerLane == adjacentLane)
        {
            Vector3 targetPosition = GetLanePosition(playerLane);
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

            HideDangerIcon();
        }
    }

    private Vector3 GetLanePosition(int lane)
    {
        switch (lane)
        {
            case 0: return leftPos.position;
            case 1: return centerPos.position;
            case 2: return rightPos.position;
            default: return centerPos.position;
        }
    }

    private void ShowDangerIcon()
    {
        dangerIcon1.SetActive(true);
        dangerIcon2.SetActive(true);

        dangerIcon1.transform.position = GetLanePosition(currentLane) + Vector3.up * 2f;
        dangerIcon2.transform.position = GetLanePosition(adjacentLane) + Vector3.up * 2f;
    }

    private void HideDangerIcon()
    {
        dangerIcon1.SetActive(false);
        dangerIcon2.SetActive(false);
    }
}
