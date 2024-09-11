using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> characters = new List<CharacterData>();

    [SerializeField]
    private List<WeaponData> weapons = new List<WeaponData>();

    [SerializeField]
    private List<WeaponEXData> weaponExes = new List<WeaponEXData>();

    public GameObject GachaSet;
    public GameObject gachaList;
    public GameObject gachaContent;

    private UserData userData;
    public string userID = "001";

    private void Awake()
    {
        Resources.UnloadUnusedAssets();
        characters = Resources.LoadAll<CharacterData>("Datas/Character").ToList();
        weapons = Resources.LoadAll<WeaponData>("Datas/Weapon/").ToList();
        weaponExes = Resources.LoadAll<WeaponEXData>("Datas/Weapon_EX/").ToList();
    }

    private void Start()
    {
        userData = DataManager.instance.userData;
    }

    public void GachaCharacter()
    {
        GachaSet.SetActive(true);
        var result = Instantiate(characters[Random.Range(0, characters.Count)]);
        userData.characters.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.characterImg;
        content[0].transform.parent = gachaList.transform;
    }

    public void GachaWeapon()
    {
        GachaSet.SetActive(true);
        var result = Instantiate(weapons[Random.Range(0, weapons.Count)]);
        userData.weapons.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
    }

    public void GachaWeaponEX()
    {
        GachaSet.SetActive(true);
        var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
        userData.weaponExes.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
    }

    public void GachaCharacter10()
    {
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(characters[Random.Range(0, characters.Count)]);
            userData.characters.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.characterImg;
            content[0].transform.parent = gachaList.transform;
        }
    }

    public void GachaWeapon10()
    {
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(weapons[Random.Range(0, weapons.Count)]);
            userData.weapons.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.weaponImg;
            content[0].transform.parent = gachaList.transform;
        }
    }

    public void GachaWeaponEX10()
    {
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
            userData.weaponExes.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.weaponImg;
            content[0].transform.parent = gachaList.transform;
        }
    }

    public void EndGacha()
    {
        GachaSet.SetActive(false);
        foreach (var obj in gachaList.GetComponentsInChildren<Image>())
        {
            Destroy(obj.gameObject);
        }
    }
}
