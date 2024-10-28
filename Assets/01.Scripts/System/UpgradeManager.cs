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
        if (!CanUpgrade(item))
            return;

        Upgrade(item, item.GetReinforceGold(), item.GetReinforceMat());
    }

    public bool CheckUpgradable(IItemData item)
    {
        return CanUpgrade(item);
    }

    private bool CanUpgrade(IItemData item)
    {
        return item.GetStarRate() < 5 &&
               checkGold(item.GetReinforceGold()) &&
               checkItemCount(item, item.GetReinforceMat());
    }


    private void Upgrade(IItemData item, int wantGold, int wantValue)
    {
        userData.money -= wantGold;

        item.SetStarRate();

        RemoveItem(item, wantValue);
    }

    private bool checkItemCount(IItemData selectItem, int wantValue)
    {
        if (selectItem is CharacterData)
        {
            return checkItemCount(selectItem, wantValue, userData.characters);
        }
        else if (selectItem is WeaponData)
        {
            return checkItemCount(selectItem, wantValue, userData.bets) ||
                   checkItemCount(selectItem, wantValue, userData.gloves);
        }
        else if (selectItem is WeaponEXData)
        {
            return checkItemCount(selectItem, wantValue, userData.weaponExes);
        }

        return false;
    }

    private bool checkItemCount<T>(IItemData selectItem, int wantValue, List<T> itemList) where T : IItemData
    {
        int sameCount = 0;
        foreach (var item in itemList)
        {
            if (!item.Equals(selectItem) && item.GetID() == selectItem.GetID())
            {
                sameCount++;
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
        if (selectItem is CharacterData)
        {
            RemoveItem(selectItem, wantValue, userData.characters);
        }
        else if (selectItem is WeaponData)
        {
            RemoveItem(selectItem, wantValue, userData.bets);
            RemoveItem(selectItem, wantValue, userData.gloves);
        }
        else if (selectItem is WeaponEXData)
        {
            RemoveItem(selectItem, wantValue, userData.weaponExes);
        }
    }

    private void RemoveItem<T>(IItemData selectItem, int wantValue, List<T> itemList) where T : IItemData
    {
        int removeCount = 0;
        for (int i = itemList.Count - 1; i >= 0 && removeCount < wantValue; i--)
        {
            if (itemList[i].GetID() == selectItem.GetID())
            {
                itemList.RemoveAt(i);
                removeCount++;
            }
        }
    }
}
