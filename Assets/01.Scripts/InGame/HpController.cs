using System.Collections;
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

    public void setHp(float maxHp)
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
