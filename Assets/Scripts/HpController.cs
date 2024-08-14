using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    public Slider HpBar;
    Collisions playerCollision;

    void Start()
    {
        playerCollision = GetComponent<Collisions>();
        setHp(100.0f);
        StartCoroutine(decressHp());
    }


    void setHp(float maxHp)
    {
        HpBar.maxValue = maxHp;
        HpBar.value = maxHp;
    }

    public void collsionObstacle()
    {
        if (HpBar.value <= 0)
            HpBar.value = 0;
        HpBar.value -= 10f;
    }

    IEnumerator decressHp()
    {
        while (true)
        {
            if (HpBar.value <= 0)
                GameManager.Instance.GameOver();
             HpBar.value -= 0.1f;

        yield return new WaitForSeconds(0.1f);
        }
    }
    public float getValue()
    {
        return HpBar.value;
    }
}
