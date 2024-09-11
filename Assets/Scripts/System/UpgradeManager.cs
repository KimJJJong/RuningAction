using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    UserData userData;

    private void Start()
    {
        userData = DataManager.instance.userData;
    }

    public void UpgradeItem(IItemData item, int wantValue, int wantGold)
    {
        if (item.GetStarRate() >= 5)
        {
            return;
        }

        if (!checkGold(wantGold))
        {
            return;
        }

        if (!checkItemCount(item, wantValue))
        {
            return;
        }

        Upgrade(item, wantGold, wantValue);
    }


    private void Upgrade(IItemData item, int wantGold, int wantValue)
    {
        userData.money -= wantGold;

        item.SetStarRate();

        RemoveItem(item, wantValue);
    }

    private bool checkItemCount(IItemData selectItem, int wantValue)
    {
        int sameCount = 0;

        if (selectItem is CharacterData)
        {
            foreach (var character in userData.characters)
            {
                if (character != selectItem && character.GetID() == selectItem.GetID())
                {
                    sameCount++;
                }
            }
        }
        else if (selectItem is WeaponData)
        {
            foreach (var weapon in userData.characters)
            {
                if (weapon != selectItem && weapon.GetID() == selectItem.GetID())
                {
                    sameCount++;
                }
            }
        }
        else if (selectItem is WeaponEXData)
        {
            foreach (var weaponEX in userData.characters)
            {
                if (weaponEX != selectItem && weaponEX.GetID() == selectItem.GetID())
                {
                    sameCount++;
                }
            }
        }

        return sameCount >= wantValue;
    }

    private bool checkGold(int wantGold)
    {
        return userData.money >= wantGold;
    }

    private void RemoveItem(IItemData selectItem, int wantValue)
    {
        int removeCount = 0;

        if (selectItem is CharacterData)
        {
            for (int i = userData.characters.Count - 1; i >= 0 && removeCount < wantValue; i--)
            {
                if (userData.characters[i] != selectItem && userData.characters[i].GetID() == selectItem.GetID())
                {
                    userData.characters.RemoveAt(i);
                    removeCount++;
                }
            }
        }
        else if (selectItem is WeaponData)
        {
            for (int i = userData.weapons.Count - 1; i >= 0 && removeCount < wantValue; i--)
            {
                if (userData.weapons[i] != selectItem && userData.weapons[i].GetID() == selectItem.GetID())
                {
                    userData.weapons.RemoveAt(i);
                    removeCount++;
                }
            }
        }
        else if (selectItem is WeaponEXData)
        {
            for (int i = userData.weaponExes.Count - 1; i >= 0 && removeCount < wantValue; i--)
            {
                if (userData.weaponExes[i] != selectItem && userData.weaponExes[i].GetID() == selectItem.GetID())
                {
                    userData.weaponExes.RemoveAt(i);
                    removeCount++;
                }
            }
        }

    }
}
