using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class ManageMenuManager : MonoBehaviour
{
    private UserData userData;

    public Image characterImage;
    public Image BetImage;
    public Image GloveImage;
    public Image ExImage;

    public List<Sprite> itemFrames = new List<Sprite>();

    public GameObject ItemList;

    public GameObject WeaponUnit;
    public GameObject ExWeaponUnit;



    private void OnEnable()
    {
        userData = DataManager.instance.userData;

        UpdateUI();
        InitItems();
    }



    void InitItems()
    {
        foreach (var childs in ItemList.GetComponentsInChildren<Button>())
        {
            Destroy(childs.gameObject);
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


}
