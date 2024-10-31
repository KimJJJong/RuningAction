using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    public Slider HpBar;
    Collisions playerCollision;

    public void StartHpControll()
    {
        playerCollision = GetComponent<Collisions>();
        SetHp(100.0f);
        StartCoroutine(DecressHp());
    }

    public void SetHp(float maxHp)
    {
        HpBar.maxValue += maxHp;
        HpBar.value = HpBar.maxValue;
    }

    public void Heal(float heal)
    {
        if (HpBar.value + heal >= HpBar.maxValue)
            HpBar.value = HpBar.maxValue;
        else
            HpBar.value += heal;
    }

    public void CollsionObstacle()
    {
        if (HpBar.value <= 0)
            HpBar.value = 0;
        HpBar.value -= 10f;
    }

    IEnumerator DecressHp()
    {
        while (true)
        {
            if (HpBar.value <= 0)
                GameManager.Instance.GameOver();
            HpBar.value -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public float GetValue()
    {
        return HpBar.value;
    }
}
