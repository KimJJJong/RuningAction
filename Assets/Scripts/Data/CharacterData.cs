using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaceterData", menuName = "Data/CharacterData")]

public class CharacterData : ScriptableObject
{
    //캐릭터 고유 ID
    [SerializeField]
    private int characterId;

    //캐릭터 이미지
    public Sprite characterImg;

    //캐릭터 이름
    public string chracterName;

    //캐릭터 설명
    public string description;
}
