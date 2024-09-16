using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUnit", menuName = "Data/WeaponData")]

public class WeaponData : ScriptableObject, IItemData
{
    public enum Rarelity {  Normal, Rare, Epic, Legendary, Mythic }
    public enum WeaponClass {  Bet, Glove }

    //무기 고유 ID
    [SerializeField]
    private int weaponId;

    [Header("# 무기 정보")]

    //무기 종류
    public WeaponClass weaponClass;

    // 무기 레어도
    public Rarelity rarelity;

    //무기 이미지
    public Sprite weaponImg;

    //무기 이름
    public string weaponName;

    //무기 설명
    [TextArea()]
    public string description;

    [Header("# 무기 업그레이드")]
    public int starRate;

    public int[] upgradeRate;

    [Header("# 업그레이드 필요 재화")]
    public int[] reinMat;
    public int[] reinGold;

    public int GetID()
    {
        return weaponId;
    }

    public void SetStarRate()
    {
        starRate++;
    }
    public int GetStarRate()
    {
        return starRate;
    }

    public int GetReinforceGold()
    {
        return reinGold[starRate - 1];
    }

    public int GetReinforceMat()
    {
        return reinMat[starRate - 1];
    }
}
