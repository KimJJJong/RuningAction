using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public List<CharacterData> characters = new List<CharacterData>();
    public List<WeaponData> weapons = new List<WeaponData>();

    public GameObject GachaSet;
    public GameObject gachaList;
    public GameObject gachaContent;

    private UserData userData;
    public string userID = "001";

    private void Awake()
    {
        userData = Resources.Load<UserData>("Datas/UserData/UserData " + userID);
    }

    public void GachaCharacter()
    {
        GachaSet.SetActive(true);
        var result = characters[Random.Range(0, characters.Count)];
        userData.characters.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.characterImg;
        content[0].transform.parent = gachaList.transform;
    }

    public void GachaWeapon()
    {
        GachaSet.SetActive(true);
        var result = weapons[Random.Range(0, weapons.Count)];
        userData.weapons.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
    }

    public void GachaCharacter10()
    {
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = characters[Random.Range(0, characters.Count)];
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
            var result = weapons[Random.Range(0, weapons.Count)];
            userData.weapons.Add(result);
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
