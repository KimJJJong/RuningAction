using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradableManager : MonoBehaviour
{
    public InUpgradeData data;

    public GameObject UpgradableContent;

    public Image UpgradalbleImage;
    public TMP_Text UpgradableName;
    public TMP_Text maxLv;

    public TMP_Text prevLv;
    public TMP_Text prevRate;

    public TMP_Text postLv;
    public TMP_Text postRate;

    public GameObject UpgradePopup;
    public GameObject RejectPopup;

    public GameObject ContentList;

    private void Start()
    {
        UpdatUI();
    }

    public void UpdatUI()
    {
        foreach (var obj in ContentList.GetComponentsInChildren<Button>())
        {
            Destroy(obj.gameObject);
        }

        foreach (var obj in DataManager.instance.userData.upgrades)
        {
            var content = Instantiate(UpgradableContent).GetComponent<UpgradableContent>();

            if (data == null)
            {
                data = obj;
            }
            content.UpdateData(obj);
            content.transform.parent = ContentList.transform;
            content.transform.localScale = Vector3.one;
        }

        UpgradalbleImage.sprite = data.upgradeImg;
        UpgradableName.text = data.upgradeName;

        maxLv.text = "Max Lv " + data.upgradeRate.Length.ToString();

        prevLv.text = "Lv " + (data.stateLv + 1).ToString();
        prevRate.text = data.upgradeRate[data.stateLv].ToString() + "%";


        if (data.upgradeRate.Length > data.stateLv + 1)
        {
            postLv.text = "Lv " + (data.stateLv + 2).ToString();
            postRate.text = data.upgradeRate[data.stateLv + 1].ToString() + "%";
        }
        else
        {
            postLv.text = "Max";
            postRate.text = "MAX";
        }

    }

    public void ShowPopup()
    {
        if (data.reinGold.Length <= data.stateLv)
        {
            return;
        }

        if (DataManager.instance.userData.money >= data.reinGold[data.stateLv])
        {
            UpgradePopup.SetActive(true);
        }
        else
        {
            RejectPopup.SetActive(true);
        }
    }

    public void UpgradeConfirm()
    {
        DataManager.instance.userData.money -= data.reinGold[data.stateLv];
        data.stateLv++;

        UpdatUI();
        FindObjectOfType<MainUIManager>().UpdateUI();

        foreach (var cont in GetComponentsInChildren<UpgradableContent>())
        {
            //cont.UpdateData(cont.GetData());
        }
    }
}
