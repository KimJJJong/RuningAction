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
        SetChracter(userData.Equipedcharacter);
        SetWeapon(userData.EquipedBet);
        SetWeapon(userData.EquipedGlove);
        SetWeaponEX(userData.EquipedEx);

        foreach (var obj in userData.upgrades)
        {
            SetSubstance(obj);
        }

        Debug.Log(userData.selectedCharacter);
        Debug.Log(userData.selectedWeaponBat);
        Debug.Log(userData.selectedWeaponGlove);
        Debug.Log(userData.selectedWeaponExes);
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
        int wClass = (int)weapon.weaponClass;    // ���� ����
        int rarelity = (int)weapon.rarelity;     // ��͵�
        int starRate = weapon.starRate;          // ��


        if (wClass == 0) // Bat
        {
            controller.eBatRank = (ERank)rarelity;
            // starRate;

        }
        else if (wClass == 1) // Glove
        {
            controller.eGloveRank = (ERank)rarelity;
            // starRate;             
        }

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

    void SetSubstance(InUpgradeData upgrade)    //����
    {
        if (upgrade.GetID() == 0)
            substance.SetHpState(upgrade.stateLv);
        else if (upgrade.GetID() == 1)
            substance.SetCoinState(upgrade.stateLv);
        else if (upgrade.GetID() == 2)
            substance.SetRushState(upgrade.stateLv);
    }
}
