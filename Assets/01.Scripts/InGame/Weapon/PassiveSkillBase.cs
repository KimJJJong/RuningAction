using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PassiveSkillBase : MonoBehaviour
{
    [System.Serializable]
    public class PassiveSkill
    {
        public SkillType skill;
        public ValueType valueType;
        public float value;
    }

    public enum ValueType
    {
        Rate,
        Amount,
        Count,
        Chance,
    }

    public enum SkillType
    {
        MaxHpIncreaseAmount,
        HpDecreaseAmount,
        HealBounusAmount,
        HealBounusAmountOnShoot,
        ObstacleDamageDecreaseRate,
        ObstacleDamageIgnoreChance,
        RunSpeedIncreaseRate,
        RunSpeedBonusScoreRate,
    }

    public List<PassiveSkill> skills;

    [Header("Stats")]
    [SerializeField]
    private float _maxHpIncreaseAmount;

    [SerializeField]
    private float _hpDecreaseAmount;

    [SerializeField]
    private float _healBounusAmount;

    [SerializeField]
    private float _healBounusAmountOnShoot;

    [SerializeField]
    private float _obstacleDamageDecreaseRate;

    [SerializeField]
    private float _obstacleDamageIgnoreChance;

    [SerializeField]
    private float _runSpeedIncreaseRate;

    [SerializeField]
    private float _runSpeedBonusScoreRate;

    [Header("Jump")]
    [SerializeField]
    private int _jumpMaxCount;

    [SerializeField]
    private float _jumpCoinDropChance;

    [SerializeField]
    private float _jumpCoinAmount;

    [SerializeField]
    private float _jumpScoreDropChance;

    [SerializeField]
    private float _jumpScoreAmount;

    [Header("Pass")]
    [SerializeField]
    private float _passCoinDropChance;

    [SerializeField]
    private float _passCoinAmount;

    [SerializeField]
    private float _passScoreDropChance;

    [SerializeField]
    private float _passScoreAmount;

    //stats
    public float maxHpIncreaseAmount
    {
        get => _maxHpIncreaseAmount;
        set => ValueChange(ref _maxHpIncreaseAmount, value);
    }
    public float hpDecreaseAmount
    {
        get => _hpDecreaseAmount;
        set => ValueChange(ref _hpDecreaseAmount, value);
    }
    public float healBounusAmount
    {
        get => _healBounusAmount;
        set => ValueChange(ref _healBounusAmount, value);
    }
    public float healBounusAmountOnShoot
    {
        get => _healBounusAmountOnShoot;
        set => ValueChange(ref _healBounusAmountOnShoot, value);
    }
    public float obstacleDamageDecreaseRate
    {
        get => _obstacleDamageDecreaseRate;
        set => ValueChange(ref _obstacleDamageDecreaseRate, value);
    }
    public float obstacleDamageIgnoreChance
    {
        get => _obstacleDamageIgnoreChance;
        set => ValueChange(ref _obstacleDamageIgnoreChance, value);
    }
    public float runSpeedIncreaseRate
    {
        get => _runSpeedIncreaseRate;
        set => ValueChange(ref _runSpeedIncreaseRate, value);
    }
    public float runSpeedBonusScoreRate
    {
        get => _runSpeedBonusScoreRate;
        set => ValueChange(ref _runSpeedBonusScoreRate, value);
    }

    //jump
    public int jumpMaxCount
    {
        get => _jumpMaxCount;
        set => ValueChange(ref _jumpMaxCount, value);
    }
    public float jumpCoinDropChance
    {
        get => _jumpCoinDropChance;
        set => ValueChange(ref _jumpCoinDropChance, value);
    }
    public float jumpCoinAmount
    {
        get => _jumpCoinAmount;
        set => ValueChange(ref _jumpCoinAmount, value);
    }
    public float jumpScoreDropChance
    {
        get => _jumpScoreDropChance;
        set => ValueChange(ref _jumpScoreDropChance, value);
    }
    public float jumpScoreAmount
    {
        get => _jumpScoreAmount;
        set => ValueChange(ref _jumpScoreAmount, value);
    }

    //pass
    public float passCoinDropChance
    {
        get => _passCoinDropChance;
        set => ValueChange(ref _passCoinDropChance, value);
    }
    public float passCoinAmount
    {
        get => _passCoinAmount;
        set => ValueChange(ref _passCoinAmount, value);
    }
    public float passScoreDropChance
    {
        get => _passScoreDropChance;
        set => ValueChange(ref _passScoreDropChance, value);
    }
    public float passScoreAmount
    {
        get => _passScoreAmount;
        set => ValueChange(ref _passScoreAmount, value);
    }

    private bool isChanged = false;

    private void Update()
    {
        if (isChanged)
            ApplyPassive();
    }

    void OnEnable()
    {
        ApplyPassive();
    }

    void OnDisable()
    {
        RevertPassive();
    }

    void ApplyPassive()
    {
        isChanged = false;
    }

    void RevertPassive()
    {
        isChanged = false;
    }

    T ValueChange<T>(ref T target, T value)
    {
        isChanged = true;
        target = value;
        return value;
    }
}
