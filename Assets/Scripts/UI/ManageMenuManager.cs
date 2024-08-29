using System.Collections;
using System.Collections.Generic;
using MH;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageMenuManager : MonoBehaviour
{
    public GameObject chrUpgradeList;
    public GameObject weaponUpgradeList;
    public GameObject innerUpgradeList;

    public Image innerUpImage;
    public TMP_Text innerText;

    public Image upgradeImage;
    public TMP_Text upgradeName;
    public TMP_Text upgradeText;

    public void ChrUpgrade()
    {
        //캐릭터의 정보를 받아와 업그레이드 팝업에 데이터 전송

        upgradeImage.sprite = chrUpgradeList.GetComponentInChildren<CharacterUnit>().GetData().characterImg;
        upgradeName.text = chrUpgradeList.GetComponentInChildren<CharacterUnit>().GetData().chracterName;
        upgradeText.text = chrUpgradeList.GetComponentInChildren<CharacterUnit>().GetData().description;
    }

    public void WeaponUpgrade()
    {
        //무기의 정보를 받아와 업그레이드 팝업에 데이터 전송

        upgradeImage.sprite = weaponUpgradeList.GetComponentInChildren<WeaponUnit>().GetData().weaponImg;
        upgradeName.text = weaponUpgradeList.GetComponentInChildren<WeaponUnit>().GetData().weaponName;
        upgradeText.text = weaponUpgradeList.GetComponentInChildren<WeaponUnit>().GetData().description;
    }

    public void InnerUpgrade()
    {
        //내실 업그레이드 정보를 나오게 함

        upgradeImage.sprite = innerUpgradeList.GetComponentInChildren<InUpgradeUnit>().GetData().upgradeImg;
        upgradeName.text = innerUpgradeList.GetComponentInChildren<InUpgradeUnit>().GetData().upgradeName;
        upgradeText.text = innerUpgradeList.GetComponentInChildren<InUpgradeUnit>().GetData().description;
    }

}
