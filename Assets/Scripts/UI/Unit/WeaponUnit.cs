using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnit : MonoBehaviour
{
    [SerializeField]
    public WeaponData weaponData;

    [SerializeField]
    int starts;

    public int num;

    private Button button;

    public Image weaponImg;

    public Sprite checkedFrame;
    public Sprite unCheckedFrame;

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
}
