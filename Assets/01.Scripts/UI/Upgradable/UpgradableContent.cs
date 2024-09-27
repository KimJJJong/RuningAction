using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradableContent : MonoBehaviour
{
    private InUpgradeData upgradeData;

    public Image UpgradableIamge;
    public TMP_Text UpgradableName;

    public TMP_Text UpgradableLevel;
    public TMP_Text UpgradableMoney;

    public void UpdateData(InUpgradeData data)
    {
        upgradeData = data;

        UpgradableIamge.sprite = data.upgradeImg;
        UpgradableName.text = data.upgradeName;
        UpgradableLevel.text = (data.stateLv + 1).ToString();
        if (data.reinGold.Length > data.stateLv)
        {
            UpgradableMoney.text = data.reinGold[data.stateLv].ToString();
        }
        else
        {
            UpgradableMoney.text = "MAX";
        }

    }

    public void SetData()
    {
        GetComponentInParent<UpgradableManager>().data = upgradeData;
        GetComponentInParent<UpgradableManager>().UpdatUI();
    }

    public InUpgradeData GetData()
    {
        return upgradeData;
    }


}
