using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Data/UserData")]
public class UserData : ScriptableObject
{
    public int userID;

    public string userName;

    public int userScore;

    public List<WeaponData> weapons = new List<WeaponData>();

    public List<CharacterData> characters = new List<CharacterData>();

    public List<InUpgradeData> upgrades = new List<InUpgradeData>();



    public int GetUserID()
    {
        return userID;
    }
}
