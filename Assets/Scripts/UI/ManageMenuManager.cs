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

    public Image innerUpImage;
    public TMP_Text innerText;

    public Image upgradeImage;
    public TMP_Text upgradeText;

    public void ChrUpgrade()
    {
        //캐릭터의 정보를 받아와 업그레이드 팝업에 데이터 전송 -> 현재는 버튼의 이미지 전송으로 대체

        upgradeImage.sprite = chrUpgradeList.GetComponentInChildren<Button>().image.sprite;
        upgradeImage.color = chrUpgradeList.GetComponentInChildren<Button>().image.color;
        upgradeText.text = chrUpgradeList.GetComponentInChildren<Button>().name;
    }

    public void WeaponUpgrade()
    {
        //무기의 정보를 받아와 업그레이드 팝업에 데이터 전송 -> 현재는 버튼의 이미지 전송으로 대체

        upgradeImage.sprite = weaponUpgradeList.GetComponentInChildren<Button>().image.sprite;
        upgradeImage.color = weaponUpgradeList.GetComponentInChildren<Button>().image.color;
        upgradeText.text = weaponUpgradeList.GetComponentInChildren<Button>().name;
    }

    public void UpgradeInner(Button btn)
    {
        //내실 업그레이드 정보를 나오게 함

        innerUpImage.sprite = btn.image.sprite;
        innerUpImage.color = btn.image.color;
        innerText.text = btn.name;
    }

}
