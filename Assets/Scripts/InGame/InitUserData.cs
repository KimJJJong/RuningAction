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
        int job = character.GetID();                            // ����
        int rarelity = (int)character.rarelity;                // ��͵�
        int starRate = character.starRate;                     // ��

        controller.eCh = (ECharacter)job;
        controller.eChRank = (ERank)rarelity;
        // ��? ���� �̾߱⸦ ������µ�?
    }

    void SetWeapon(WeaponData weapon)       //�� �Ḹ.... bat�ϰ� glove�ϰ� �����̿�������??? �Ф�
    {
        int wClass = (int)weapon.rarelity;    // ���� ����
        int rarelity = (int)weapon.rarelity;     // ��͵�
        int starRate = weapon.starRate;          // ��

       // controller.eMainWeapons = (ECharacter)wClass;
       // controller.eMainWeaponRank = (ERank)rarelity;
        // ��...�̶�...�Ф�

    }

    void SetWeaponEX(WeaponEXData weaponEX)     // InGame������ Equipment
    {
        int wClass = (int)weaponEX.weaponClass;    // ���� ����
        int rarelity = (int)weaponEX.rarelity;     // ��͵�
        int starRate = weaponEX.starRate;          // ��

        equipment.equip = (EEquipment)wClass;
        equipment.erank = (ERank)rarelity;
        //��������������������������.


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
