using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    private GameManager gameManager;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void PlayGoalText()
    {
        StartCoroutine(GoalTextCor());
    }    

    private IEnumerator GoalTextCor()
    {
        TextMeshProUGUI goalText = gameManager.collisions.goalText;
        goalText.enabled = true;
        goalText.GetComponent<DOTweenAnimation>().DOPlay();

        yield return new WaitForSeconds(2f);
        goalText.enabled = false;
    }
}
