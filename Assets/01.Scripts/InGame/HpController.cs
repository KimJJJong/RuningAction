using System;
using System.Collections;
using Blobcreate.Universal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    private static event BaseActionWithParams onHeal;
    private static event BaseActionWithParams onDamage;
    private static event BaseAction onHpZero;

    //public static UnityEvent<GameObject> OnHpZero = new UnityEvent<GameObject>();

    public static void Heal(GameObject player, float value)
    {
        if (!player.CompareTag("Player"))
        {
            Debug.LogError("Heal target Is Not Player");
            return;
        }
        onHeal?.Invoke(player, value);
    }

    public static void Damage(GameObject player, float value)
    {
        if (!player.CompareTag("Player"))
        {
            Debug.LogError("Damage target Is Not Player");
            return;
        }
        onDamage?.Invoke(player, value);
    }

    public static void OnHpZero(BaseAction action)
    {
        onHpZero += action;
    }

    public Slider HpBar;

    public float maxHp = 0f;

    [SerializeField]
    private float _hp;
    public float hp
    {
        get { return _hp; }
    }

    private float hpReductionPerSecond = 1f;

    public void Awake()
    {
        onHeal = null;
        onDamage = null;
        onHpZero = null;
    }

    public void Start()
    {
        InitHpBar();

        GameManager.OnGameStateChange.AddListener(
            (state) =>
            {
                if (state == GameState.Playing)
                {
                    StartReduceHpOverTime();
                }
                else
                {
                    StopReduceHpOverTime();
                }
            }
        );
        GameObject gameObject = this.gameObject;
        onHeal += (player, value) =>
        {
            if (player == gameObject)
                UpdateHp(+value);
        };

        onDamage += (player, value) =>
        {
            if (player == gameObject)
                UpdateHp(-value);
        };
    }

    public void InitHpBar()
    {
        _hp = maxHp;

        if (HpBar == null)
            return;
        HpBar.maxValue = maxHp;
        HpBar.value = HpBar.maxValue;
    }

    public void StartReduceHpOverTime()
    {
        StartCoroutine(ReduceHealthOverTime_Cor());
    }

    public void StopReduceHpOverTime()
    {
        StopCoroutine(ReduceHealthOverTime_Cor());
    }

    public IEnumerator ReduceHealthOverTime_Cor()
    {
        while (_hp > 0)
        {
            yield return new WaitForSeconds(0.1f);
            float value = hpReductionPerSecond * 0.1f;
            UpdateHp(-value);

            if (HpBar != null)
                HpBar.value = _hp;
        }

        onHpZero?.Invoke(this.gameObject);
    }

    private void UpdateHp(float value)
    {
        _hp = Math.Min(_hp + value, maxHp);

        if (_hp > 0)
            return;

        //onHpZero?.Invoke(this.gameObject);
    }

    public delegate void BaseAction(GameObject gameObject);
    private delegate void BaseActionWithParams(GameObject gameObject, float value);
}
