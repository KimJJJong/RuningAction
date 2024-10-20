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

    public void UpgradeItem(IItemData item)
    {
        if (item.GetStarRate() >= 5)
        {
            return;
        }

        if (!checkGold(item.GetReinforceGold()))
        {
            return;
        }

        if (!checkItemCount(item, item.GetReinforceMat()))
        {
            return;
        }

        Upgrade(item, item.GetReinforceGold(), item.GetReinforceMat());
    }

    public bool CheckUpgradable(IItemData item)
    {
        if (item.GetStarRate() >= 5)
        {
            return false;
        }

        if (!checkGold(item.GetReinforceGold()))
        {
            return false;
        }

        if (!checkItemCount(item, item.GetReinforceMat()))
        {
            return false;
        }

        return true;
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
            foreach (var weapon in userData.bets)
            {
                if (weapon != selectItem && weapon.GetID() == selectItem.GetID())
                {
                    sameCount++;
                }
            }
            foreach (var weapon in userData.gloves)
            {
                if (weapon != selectItem && weapon.GetID() == selectItem.GetID())
                {
                    sameCount++;
                }
            }
        }
        else if (selectItem is WeaponEXData)
        {
            foreach (var weaponEX in userData.weaponExes)
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
            for (int i = userData.bets.Count - 1; i >= 0 && removeCount < wantValue; i--)
            {
                if (userData.bets[i] != selectItem && userData.bets[i].GetID() == selectItem.GetID())
                {
                    userData.bets.RemoveAt(i);
                    removeCount++;
                }
            }

            for (int i = userData.gloves.Count - 1; i >= 0 && removeCount < wantValue; i--)
            {
                if (userData.gloves[i] != selectItem && userData.gloves[i].GetID() == selectItem.GetID())
                {
                    userData.gloves.RemoveAt(i);
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
