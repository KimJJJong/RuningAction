using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    public WeaponData weaponData;

    public int num;

    private Button button;

    public Image weaponImg;

    public Sprite gloveImg;
    public Sprite betImg;
    public Image SymbolImg;

    public GameObject[] StarList;

    private void Start()
    {
        UpgradeUnit();
        button = GetComponent<Button>();
        weaponImg.sprite = weaponData.weaponImg;

        if (weaponData.weaponClass == WeaponData.WeaponClass.Bet)
        {
            SymbolImg.sprite = betImg;
        }
        else
        {
            SymbolImg.sprite = gloveImg;
        }
    }

    public void CheckThis()
    {
        if (weaponData.weaponClass == WeaponData.WeaponClass.Bet)
        {
            DataManager.instance.userData.EquipedBet = weaponData;
        }
        else
        {
            DataManager.instance.userData.EquipedGlove = weaponData;
        }
        FindObjectOfType<ManageMenuManager>().UpdateUI();
    }

    public void SetData(WeaponData data)
    {
        weaponData = data;
    }

    public IItemData GetData()
    {
        return weaponData;
    }

    public void UpgradeUnit()
    {
        for (int i = 0; i < weaponData.starRate; i++)
        {
            StarList[i].gameObject.SetActive(true);
        }
    }
}
