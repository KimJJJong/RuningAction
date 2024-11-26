using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealEvent : UnityEngine.Events.UnityEvent<float> { }

public class HpController : MonoBehaviour
{
    public static HealEvent OnHeal = new HealEvent();
    public Slider HpBar;

    public void Start()
    {
        GameManager.OnGameStateChange.AddListener(
            (state) =>
            {
                if (state == GameState.Playing)
                {
                    StartHpControll();
                }
            }
        );

        OnHeal.AddListener(
            (heal) =>
            {
                Heal(heal);
            }
        );
    }

    public void StartHpControll()
    {
        SetHp(1000.0f);
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
