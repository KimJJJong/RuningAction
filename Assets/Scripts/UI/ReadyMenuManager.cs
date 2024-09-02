using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyMenuManager : MonoBehaviour
{
    public Image charImage;
    public Image weaponImage;

    private UserData userData;
    public string userID = "001";

    public GameObject characterSelectList;
    public GameObject weaponSelectList;
    public GameObject upgraderSelectList;

    public GameObject characterSelectButton;
    public GameObject weaponSelectButton;
    public GameObject upgradeImg;

    private void Awake()
    {
        userData = Resources.Load<UserData>("Datas/UserData/UserData " + userID);
    }

    private void Start()
    {
        foreach (var chr in userData.characters)
        {
            var btn = Instantiate(characterSelectButton);
            btn.GetComponent<Button>().image.sprite = chr.characterImg;
            btn.GetComponent<Button>().onClick.AddListener(() => UpdateCharImage(btn.GetComponent<Button>()));
            btn.transform.parent = characterSelectList.transform;
        }
        foreach (var weapon in userData.weapons)
        {
            var btn = Instantiate(weaponSelectButton);
            btn.GetComponent<Button>().image.sprite = weapon.weaponImg;
            btn.GetComponent<Button>().onClick.AddListener(() => UpdateWeaponImage(btn.GetComponent<Button>()));
            btn.transform.parent = weaponSelectList.transform;
        }
        foreach (var upgrade in userData.upgrades)
        {
            var img = Instantiate(upgradeImg);
            img.GetComponent<Image>().sprite = upgrade.upgradeImg;
            img.transform.parent = upgraderSelectList.transform;
        }

    }

    public void UpdateCharImage(Button btn)
    {
        charImage.sprite = btn.image.sprite;
        charImage.color = btn.image.color;
    }

    public void UpdateWeaponImage(Button btn)
    {
        weaponImage.sprite = btn.image.sprite;
        weaponImage.color = btn.image.color;
    }
}
