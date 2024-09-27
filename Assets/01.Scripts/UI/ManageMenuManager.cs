using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class ManageMenuManager : MonoBehaviour
{
    private UserData userData;

    public GameObject upgradePopup;

    public Image characterImage;
    public Image BetImage;
    public Image GloveImage;
    public Image ExImage;

    public List<Sprite> itemFrames = new List<Sprite>();

    public GameObject ItemList;
    public GameObject CharacterList;

    public GameObject CharacterUnit;
    public GameObject WeaponUnit;
    public GameObject ExWeaponUnit;


    private void Start()
    {

    }

    private void OnEnable()
    {
        userData = DataManager.instance.userData;

        Init();
    }

    public void Init()
    {
        UpdateUI();
        InitItems();
    }


    void InitItems()
    {
        foreach (var childs in ItemList.GetComponentsInChildren<Button>())
        {
            Destroy(childs.gameObject);
        }
        foreach (var childs in CharacterList.GetComponentsInChildren<Button>())
        {
            Destroy(childs.gameObject);
        }

        foreach (var obj in userData.characters)
        {
            var unit = Instantiate(CharacterUnit).GetComponent<CharacterUnit>();
            unit.SetData(obj);
            unit.gameObject.transform.parent = CharacterList.transform;
            unit.gameObject.transform.localScale = Vector3.one;
            unit.GetComponent<Image>().sprite = itemFrames[obj.GetRarelity()];
        }

        foreach (var obj in userData.bets)
        {
            var unit = Instantiate(WeaponUnit).GetComponent<WeaponUnit>();
            unit.SetData(obj);
            unit.gameObject.transform.parent = ItemList.transform;
            unit.gameObject.transform.localScale = Vector3.one;
            unit.GetComponent<Image>().sprite = itemFrames[obj.GetRarelity()];
        }
        foreach (var obj in userData.gloves)
        {
            var unit = Instantiate(WeaponUnit).GetComponent<WeaponUnit>();
            unit.SetData(obj);
            unit.gameObject.transform.parent = ItemList.transform;
            unit.gameObject.transform.localScale = Vector3.one;
            unit.GetComponent<Image>().sprite = itemFrames[obj.GetRarelity()];
        }
        foreach (var obj in userData.weaponExes)
        {
            var unit = Instantiate(ExWeaponUnit).GetComponent<WeaponEXUnit>();
            unit.SetData(obj);
            unit.gameObject.transform.parent = ItemList.transform;
            unit.gameObject.transform.localScale = Vector3.one;
            unit.GetComponent<Image>().sprite = itemFrames[obj.GetRarelity()];
        }
    }

    public void UpdateUI()
    {
        characterImage.sprite = userData.Equipedcharacter.characterImg;
        BetImage.sprite = userData.EquipedBet.weaponImg;
        GloveImage.sprite = userData.EquipedGlove.weaponImg;
        ExImage.sprite = userData.EquipedEx.weaponImg;
    }

    public void UpgradeChar()
    {
        upgradePopup.SetActive(true);
        upgradePopup.GetComponent<UpgradePopupManager>().SetCharData(userData.Equipedcharacter);
    }

    public void UpgradeBet()
    {
        upgradePopup.SetActive(true);
        upgradePopup.GetComponent<UpgradePopupManager>().SetWeaponData(userData.EquipedBet);
    }

    public void UpgradeGlove()
    {
        upgradePopup.SetActive(true);
        upgradePopup.GetComponent<UpgradePopupManager>().SetWeaponData(userData.EquipedGlove);
    }

    public void UpgradeEX()
    {
        upgradePopup.SetActive(true);
        upgradePopup.GetComponent<UpgradePopupManager>().SetExData(userData.EquipedEx);
    }
}
