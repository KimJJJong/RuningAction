using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InUpgradeData", menuName = "Data/InUpgradeData")]

public class InUpgradeData : ScriptableObject
{
    //내실 고유 ID
    [SerializeField]
    private int upgradeId;

    //내실 이미지
    public Sprite upgradeImg;

    public string upgradeName;

    //내실 설명
    public string description;

    [Header("# 내실 업그레이드")]
    public int stateLv;
    public int[] upgradeRate;

    [Header("# 업그레이드 필요 재화")]
    public int[] reinGold;

    public int GetID()
    {
        return upgradeId;
    }
}
