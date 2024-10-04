using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UpgradePopupManager : MonoBehaviour
{
    UserData userData;
    private UpgradeManager upgrade;
    public ManageMenuManager manage;
    public MainUIManager mainUi;

    private IItemData upgradeItem;

    public GameObject confirmPopup;
    public GameObject rejectPopup;

    public TMP_Text curDesc;
    public TMP_Text aftDesc;

    public TMP_Text GoldText;
    public Image MatImage;
    public TMP_Text MatText;


    private void Start()
    {
        upgrade = GetComponent<UpgradeManager>();
    }

    public void SetCharData(CharacterData itemData)
    {
        userData = DataManager.instance.userData;

        upgradeItem = itemData;

        curDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate - 1]);
        if (itemData.upgradeRate.Length > itemData.starRate)
        {
            aftDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate]);
        }
        else
        {
            aftDesc.text = "Max Upgrade";
        }

        MatImage.sprite = itemData.characterImg;

        int haves = userData.characters.Count(item => item.GetID() == itemData.GetID() && item.GetStarRate() == itemData.GetStarRate()) - 1;
        int wantsMat = itemData.reinMat[itemData.starRate - 1];
        int wantGold = itemData.reinGold[itemData.starRate - 1];
        GoldText.text = userData.money + " / " + wantGold;
        MatText.text = haves.ToString() + " / " + wantsMat.ToString();

        if (haves >= wantsMat && userData.money >= wantGold)
        {
            GoldText.color = Color.white;
            MatText.color = Color.white;
        }
        else
        {
            if (haves < wantsMat)
            {
                MatText.color = Color.red;
            }
            if (userData.money < wantGold)
            {
                GoldText.color = Color.red;
            }
        }

    }

    public void SetWeaponData(WeaponData itemData)
    {
        userData = DataManager.instance.userData;

        upgradeItem = itemData;

        curDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate - 1]);
        if (itemData.upgradeRate.Length > itemData.starRate)
        {
            aftDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate]);
        }
        else
        {
            aftDesc.text = "Max Upgrade";
        }

        MatImage.sprite = itemData.weaponImg;

        int haves = 0;

        if (itemData.weaponClass == WeaponData.WeaponClass.Bet)
        {
            haves = userData.bets.Count(item => item.GetID() == itemData.GetID() && item.GetStarRate() == itemData.GetStarRate()) - 1;
        }
        else
        {
            haves = userData.gloves.Count(item => item.GetID() == itemData.GetID() && item.GetStarRate() == itemData.GetStarRate()) - 1;
        }

        int wantsMat = itemData.reinMat[itemData.starRate - 1];
        int wantGold = itemData.reinGold[itemData.starRate - 1];
        GoldText.text = userData.money + " / " + wantGold;
        MatText.text = haves.ToString() + " / " + wantsMat.ToString();

        if (haves >= wantsMat && userData.money >= wantGold)
        {
            GoldText.color = Color.white;
            MatText.color = Color.white;
        }
        else
        {
            if (haves < wantsMat)
            {
                MatText.color = Color.red;
            }
            if (userData.money < wantGold)
            {
                GoldText.color = Color.red;
            }
        }
    }

    public void SetExData(WeaponEXData itemData)
    {
        userData = DataManager.instance.userData;

        upgradeItem = itemData;

        curDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate - 1]);
        if (itemData.upgradeRate.Length > itemData.starRate)
        {
            aftDesc.text = string.Format(itemData.description, itemData.upgradeRate[itemData.starRate]);
        }
        else
        {
            aftDesc.text = "Max Upgrade";
        }

        MatImage.sprite = itemData.weaponImg;

        int haves = userData.weaponExes.Count(item => item.GetID() == itemData.GetID() && item.GetStarRate() == itemData.GetStarRate()) - 1;
        int wantsMat = itemData.reinMat[itemData.starRate - 1];
        int wantGold = itemData.reinGold[itemData.starRate - 1];
        GoldText.text = userData.money + " / " + wantGold;
        MatText.text = haves.ToString() + " / " + wantsMat.ToString();

        if (haves >= wantsMat && userData.money >= wantGold)
        {
            GoldText.color = Color.white;
            MatText.color = Color.white;
        }
        else
        {
            if (haves < wantsMat)
            {
                MatText.color = Color.red;
            }
            if (userData.money < wantGold)
            {
                GoldText.color = Color.red;
            }
        }
    }

    public void OpenPanel()
    {
        if (upgrade.CheckUpgradable(upgradeItem))
        {
            confirmPopup.SetActive(true);
        }
        else
        {
            rejectPopup.SetActive(true);
        }
    }

    public void UpradeConfirm()
    {
        upgrade.UpgradeItem(upgradeItem);
        manage.Init();
        mainUi.UpdateUI();
    }
}
