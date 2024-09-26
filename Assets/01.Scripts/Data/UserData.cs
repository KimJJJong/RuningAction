using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Data/UserData")]
public class UserData : ScriptableObject
{
    public int userID;

    public string userName;

    public int userScore;

    public int money;

    //public List<WeaponData> weapons = new List<WeaponData>();
    public List<WeaponData> bets = new List<WeaponData>();
    public List<WeaponData> gloves = new List<WeaponData>();

    public List<WeaponEXData> weaponExes = new List<WeaponEXData>();

    public List<CharacterData> characters = new List<CharacterData>();

    public List<InUpgradeData> upgrades = new List<InUpgradeData>();

    public CharacterData Equipedcharacter;
    public WeaponData EquipedBet;
    public WeaponData EquipedGlove;
    public WeaponEXData EquipedEx;

    public int selectedCharacter;

    public int selectedWeaponBat;

    public int selectedWeaponGlove;

    public int selectedWeaponExes;


    public int GetUserID()
    {
        return userID;
    }

    private void OnValidate()
    {
        selectedCharacter = 0;
        selectedWeaponBat = 0;
        selectedWeaponExes = 0;
        selectedWeaponGlove = 0;

        Equipedcharacter = characters[selectedCharacter];
        EquipedBet = bets[selectedWeaponBat];
        EquipedGlove = gloves[selectedWeaponGlove];
        EquipedEx = weaponExes[selectedWeaponExes];
    }
}
