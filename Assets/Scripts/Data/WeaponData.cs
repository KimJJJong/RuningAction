using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUnit", menuName = "Data/WeaponData")]

public class WeaponData : ScriptableObject
{
    //무기 고유 ID
    [SerializeField]
    private int weaponId;

    //무기 이미지
    public Sprite weaponImg;

    //무기 이름
    public string weaponName;

    //무기 설명
    public string description;
}
