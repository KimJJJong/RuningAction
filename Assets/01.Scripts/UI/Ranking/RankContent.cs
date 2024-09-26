using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankContent : MonoBehaviour
{
    public int score;
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    private void Update()
    {
        rankText.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public void SetData(UserData data)
    {
        nameText.text = data.userName;
        scoreText.text = data.userScore.ToString();
        score = data.userScore;
    }

    public void SetUser()
    {
        rankText.color = Color.magenta;
        nameText.color = Color.magenta;
        scoreText.color = Color.magenta;
    }

}
