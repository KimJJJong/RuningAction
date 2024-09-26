using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEXUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    public WeaponEXData weaponData;

    public int num;

    private Button button;

    public Image weaponImg;

    public Sprite checkedFrame;
    public Sprite unCheckedFrame;

    public GameObject[] StarList;

    private void Start()
    {
        UpgradeUnit();
        button = GetComponent<Button>();
        weaponImg.sprite = weaponData.weaponImg;
    }

    void Update()
    {

    }

    public void CheckThis()
    {
        DataManager.instance.userData.EquipedEx = weaponData;
        FindObjectOfType<ManageMenuManager>().UpdateUI();
    }

    public void SetData(WeaponEXData data)
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
