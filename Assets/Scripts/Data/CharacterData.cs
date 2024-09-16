using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaceterData", menuName = "Data/CharacterData")]

public class CharacterData : ScriptableObject, IItemData
{
    public enum Rarelity {  Normal, Rare, Epic, Legendary, Mythic }

    //캐릭터 고유 ID
    [SerializeField]
    private int characterId;

    [Header("# 캐릭터 정보")]

    //캐릭터 레어도
    public Rarelity rarelity;

    //캐릭터 이미지
    public Sprite characterImg;

    //캐릭터 이름
    public string chracterName;

    //캐릭터 설명
    [TextArea()]
    public string description;

    [Header("# 캐릭터 업그레이드")]
    public int starRate;
    public int[] upgradeRate;

    [Header("# 업그레이드 필요 재화")]
    public int[] reinMat;
    public int[] reinGold;

    public int GetID()
    {
        return characterId;
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
