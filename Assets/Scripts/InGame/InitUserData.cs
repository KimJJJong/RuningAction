using UnityEngine;

public class InitUserData : MonoBehaviour
{
    PlayerController controller;
    StrengthenSubstance substance;
    Equipment equipment;
    UserData userData;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        equipment = GetComponent<Equipment>();
        substance = GetComponent<StrengthenSubstance>();
        userData = DataManager.instance.userData;


        SetState();
    }

    void SetState()
    {
        SetChracter(userData.characters[userData.selectedCharacter]);
        SetWeapon(userData.weapons[userData.selectedWeapon]);
        SetWeaponEX(userData.weaponExes[userData.selectedWeaponExes]);
        foreach (var obj in userData.upgrades)
        {
            SetSubstance(obj);
        }
    }


    void SetChracter(CharacterData character)
    {
        int job = character.GetID();                            // 직업
        int rarelity = (int)character.rarelity;                // 희귀도
        int starRate = character.starRate;                     // 별

        controller.eCh = (ECharacter)job;
        controller.eChRank = (ERank)rarelity;
        // 별? 구현 이야기를 못들었는데?
    }

    void SetWeapon(WeaponData weapon)       //아 잠만.... bat하고 glove하고 각각이였던거임??? ㅠㅠ
    {
        int wClass = (int)weapon.rarelity;    // 무기 종류
        int rarelity = (int)weapon.rarelity;     // 희귀도
        int starRate = weapon.starRate;          // 별

       // controller.eMainWeapons = (ECharacter)wClass;
       // controller.eMainWeaponRank = (ERank)rarelity;
        // 별...이라...ㅠㅠ

    }

    void SetWeaponEX(WeaponEXData weaponEX)     // InGame에서는 Equipment
    {
        int wClass = (int)weaponEX.weaponClass;    // 무기 종류
        int rarelity = (int)weaponEX.rarelity;     // 희귀도
        int starRate = weaponEX.starRate;          // 별

        equipment.equip = (EEquipment)wClass;
        equipment.erank = (ERank)rarelity;
        //별ㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹ.


    }

    void SetSubstance(InUpgradeData upgrade)
    {
        if (upgrade.GetID() == 0)
            substance.SetHpState(upgrade.stateLv);
        else if (upgrade.GetID() == 1)
            substance.SetCoinState(upgrade.stateLv);
        else if (upgrade.GetID() == 2)
            substance.SetRushState(upgrade.stateLv);
    }
}
