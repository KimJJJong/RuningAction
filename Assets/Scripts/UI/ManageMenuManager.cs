using System.Collections;
using System.Collections.Generic;
using MH;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class ManageMenuManager : MonoBehaviour
{
    public GameObject chrUpgradeList;
    public GameObject weaponUpgradeList;
    public GameObject weaponExUpgradeList;
    public GameObject innerUpgradeList;

    public GameObject chrUnit;
    public GameObject weaponUnit;
    public GameObject weaponExUnit;
    public GameObject upgradeUnit;


    public IItemData itemData;
    public IUnit unitData;
    public Image upgradeImage;
    public TMP_Text upgradeName;
    public TMP_Text upgradeText;

    public TMP_Text beforeText;
    public TMP_Text afterText;

    private UserData userData;

    public TMP_Text goldText;

    public GameObject confirmPanel;
    public GameObject warnPanel;

    public TMP_Text reinforceText;
    public Image reinforceImg;

    public TMP_Text reinforceGoldText;

    public GameObject upgradeButton;

    private int haves;
    private int wantsMat;
    private int wantsGold;
    public string userID = "001";

    private void Start()
    {
        userData = DataManager.instance.userData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        goldText.text = string.Format("{0:#,0}", DataManager.instance.userData.money);

        foreach (var it in chrUpgradeList.GetComponentsInChildren<CharacterUnit>())
        {
            Destroy(it.gameObject);
        }
        foreach (var it in weaponUpgradeList.GetComponentsInChildren<CharacterUnit>())
        {
            Destroy(it.gameObject);
        }
        foreach (var it in weaponExUpgradeList.GetComponentsInChildren<CharacterUnit>())
        {
            Destroy(it.gameObject);
        }

        foreach (var chr in userData.characters)
        {
            var unit = Instantiate(chrUnit).GetComponent<CharacterUnit>();
            unit.SetData(chr);
            unit.transform.parent = chrUpgradeList.transform;
            unit.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach (var weapon in userData.weapons)
        {
            var unit = Instantiate(weaponUnit).GetComponent<WeaponUnit>();
            unit.weaponData = weapon;
            unit.transform.parent = weaponUpgradeList.transform;
            unit.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach (var weaponEx in userData.weaponExes)
        {
            var unit = Instantiate(weaponExUnit).GetComponent<WeaponEXUnit>();
            unit.weaponData = weaponEx;
            unit.transform.parent = weaponExUpgradeList.transform;
            unit.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach (var upgrade in userData.upgrades)
        {
            var unit = Instantiate(upgradeUnit).GetComponent<InUpgradeUnit>();
            unit.upgradeData = upgrade;
            unit.transform.parent = innerUpgradeList.transform;
            unit.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ChrUpgrade()
    {
        //캐릭터의 정보를 받아와 업그레이드 팝업에 데이터 전송
        var data = chrUpgradeList.GetComponentInChildren<CharacterUnit>().GetData();
        unitData = chrUpgradeList.GetComponentInChildren<CharacterUnit>();

        upgradeImage.sprite = data.characterImg;
        upgradeName.text = data.chracterName;
        upgradeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);

        beforeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);
        if (data.starRate > data.upgradeRate.Length)
        {
            afterText.text = "Max Upgraded";
        }
        else
        {
            afterText.text = string.Format(data.description, data.upgradeRate[data.starRate]);
        }

        itemData = data;

        reinforceImg.sprite = data.characterImg;

        haves = userData.characters.Count(item => (item.GetID() == data.GetID() && item.GetStarRate() == data.GetStarRate()));
        wantsMat = data.reinMat[data.starRate - 1];
        wantsGold = data.reinGold[data.starRate - 1];
        reinforceText.text = haves.ToString() + " / " + wantsMat.ToString();
        reinforceGoldText.text = userData.money + " / " + wantsGold;

        if (haves >= wantsMat && userData.money >= wantsGold)
        {
            upgradeButton.SetActive(true);
            reinforceText.color = Color.black;
        }
        else
        {
            upgradeButton.SetActive(false);
            if (haves < wantsMat)
            {
                reinforceText.color = Color.red;
            }
            else if (userData.money < wantsGold)
            {
                reinforceGoldText.color = Color.red;
            }

        }

    }

    public void WeaponUpgrade()
    {
        //무기의 정보를 받아와 업그레이드 팝업에 데이터 전송
        var data = weaponUpgradeList.GetComponentInChildren<WeaponUnit>().GetData();
        unitData = weaponUpgradeList.GetComponentInChildren<WeaponUnit>();

        upgradeImage.sprite = data.weaponImg;
        upgradeName.text = data.weaponName;
        upgradeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);

        beforeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);
        if (data.starRate > data.upgradeRate.Length)
        {
            afterText.text = "Max Upgraded";
        }
        else
        {
            afterText.text = string.Format(data.description, data.upgradeRate[data.starRate]);
        }

        itemData = data;

        reinforceImg.sprite = data.weaponImg;
        haves = userData.weapons.Count(item => (item.GetID() == data.GetID() && item.GetStarRate() == data.GetStarRate()));
        wantsMat = data.reinMat[data.starRate - 1];
        wantsGold = data.reinGold[data.starRate - 1];
        reinforceText.text = haves.ToString() + " / " + wantsMat.ToString();
        reinforceGoldText.text = userData.money + " / " + wantsGold;

        if (haves >= wantsMat && userData.money >= wantsGold)
        {
            upgradeButton.SetActive(true);
            reinforceText.color = Color.black;
        }
        else
        {
            upgradeButton.SetActive(false);
            if (haves < wantsMat)
            {
                reinforceText.color = Color.red;
            }
            else if (userData.money < wantsGold)
            {
                reinforceGoldText.color = Color.red;
            }
        }
    }

    public void WeaponExUPgrade()
    {
        var data = weaponExUpgradeList.GetComponentInChildren<WeaponEXUnit>().GetData();
        unitData = weaponExUpgradeList.GetComponentInChildren<WeaponEXUnit>();

        upgradeImage.sprite = data.weaponImg;
        upgradeName.text = data.weaponName;
        upgradeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);

        beforeText.text = string.Format(data.description, data.upgradeRate[data.starRate - 1]);
        if (data.starRate > data.upgradeRate.Length)
        {
            afterText.text = "Max Upgraded";
        }
        else
        {
            afterText.text = string.Format(data.description, data.upgradeRate[data.starRate]);
        }

        itemData = data;

        reinforceImg.sprite = data.weaponImg;
        haves = userData.weaponExes.Count(item => (item.GetID() == data.GetID() && item.GetStarRate() == data.GetStarRate()));
        wantsMat = data.reinMat[data.starRate - 1];
        wantsGold = data.reinGold[data.starRate - 1];
        reinforceText.text = haves.ToString() + " / " + wantsMat.ToString();
        reinforceGoldText.text = userData.money + " / " + wantsGold;

        if (haves >= wantsMat && userData.money >= wantsGold)
        {
            upgradeButton.SetActive(true);
            reinforceText.color = Color.black;
        }
        else
        {
            upgradeButton.SetActive(false);
            if (haves < wantsMat)
            {
                reinforceText.color = Color.red;
            }
            else if (userData.money < wantsGold)
            {
                reinforceGoldText.color = Color.red;
            }
        }
    }

    public void InnerUpgrade()
    {
        //내실 업그레이드 정보를 나오게 함
        var data = innerUpgradeList.GetComponentInChildren<InUpgradeUnit>().GetData();

        upgradeImage.sprite = data.upgradeImg;
        upgradeName.text = data.upgradeName;
        upgradeText.text = data.description;
    }

    public void Upgrade()
    {
        GetComponent<UpgradeManager>().UpgradeItem(itemData, itemData.GetReinforceMat(), itemData.GetReinforceGold());
        UpdateUI();
    }
}
