using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    private Animator animator;

    private void Start()
    {
        shotOnGoal = transform.parent.GetComponent<ShotOnGoal>();
        animator = GetComponent<Animator>();
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
            float targetX = targetPosition.x;
            float currentX = transform.position.x;

            if (targetX < currentX)
            {
                animator.Play("MoveLeft");
            }
            else if (targetX > currentX)
            {
                animator.Play("MoveRight");
            }
            else
            {
                animator.Play("Catch");
            }

            transform.DOMoveX(targetX, 0.5f).SetEase(Ease.InOutQuad);

            HideDangerIcon();
        }
        else
        {
            Vector3 targetPosition = GetLanePosition(playerLane);
            float targetX = targetPosition.x;

            transform.DOMoveX(targetX, 0.5f).SetEase(Ease.InOutQuad);

            animator.Play("MoveRight");

            transform.DOMoveX(targetX, 0.5f).SetEase(Ease.InOutQuad);
        }
    }

    private Vector3 GetLanePosition(int lane)
    {
        switch (lane)
        {
            case 0:
                return leftPos.position;
            case 2:
                return rightPos.position;
            default:
                return centerPos.position;
        }
    }

    private void ShowDangerIcon()
    {
        dangerIcon1.SetActive(true);
        dangerIcon2.SetActive(true);

        dangerIcon1.transform.position = GetLanePosition(currentLane) + Vector3.up * 3f;
        dangerIcon2.transform.position = GetLanePosition(adjacentLane) + Vector3.up * 3f;
    }

    private void HideDangerIcon()
    {
        dangerIcon1.SetActive(false);
        dangerIcon2.SetActive(false);
    }
}
