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

    public Sprite checkedFrame;
    public Sprite unCheckedFrame;

    public GameObject[] StarList;

    private void Start()
    {
        button = GetComponent<Button>();
        weaponImg.sprite = weaponData.weaponImg;
    }

    void Update()
    {
        if (transform.GetSiblingIndex() == 0)
        {
            button.image.sprite = checkedFrame;
        }
        else
        {
            button.image.sprite = unCheckedFrame;
        }
    }

    public void CheckThis()
    {
        transform.SetAsFirstSibling();
    }

    public void SetData(WeaponData data)
    {
        weaponData = data;
    }

    public WeaponData GetData()
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
