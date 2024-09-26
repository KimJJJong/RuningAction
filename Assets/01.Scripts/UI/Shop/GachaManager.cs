using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField]
    private List<GameObject> gachaEffects = new List<GameObject>();

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
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        GachaSet.SetActive(true);
        var result = Instantiate(characters[Random.Range(0, characters.Count)]);
        userData.characters.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.characterImg;
        content[0].transform.parent = gachaList.transform;
        content[0].transform.localScale = Vector3.one;
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
    }

    public void GachaWeapon()
    {
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        GachaSet.SetActive(true);
        var result = Instantiate(weapons[Random.Range(0, weapons.Count)]);
        if (result.weaponClass == WeaponData.WeaponClass.Bet)
        {
            userData.bets.Add(result);
        }
        else
        {
            userData.gloves.Add(result);
        }
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
        content[0].transform.localScale = Vector3.one;
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
    }

    public void GachaWeaponEX()
    {
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        GachaSet.SetActive(true);
        var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
        userData.weaponExes.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
        content[0].transform.localScale = Vector3.one;
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
    }

    public void GachaCharacter10()
    {
        StartCoroutine("gachaChar10");
    }

    IEnumerator gachaChar10()
    {
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(characters[Random.Range(0, characters.Count)]);
            userData.characters.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.characterImg;
            content[0].transform.parent = gachaList.transform;
            content[0].transform.localScale = Vector3.one;
            Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void GachaWeapon10()
    {
        StartCoroutine("gachaWea10");
    }

    IEnumerator gachaWea10()
    {
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(weapons[Random.Range(0, weapons.Count)]);
            if (result.weaponClass == WeaponData.WeaponClass.Bet)
            {
                userData.bets.Add(result);
            }
            else
            {
                userData.gloves.Add(result);
            }
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.weaponImg;
            content[0].transform.parent = gachaList.transform;
            content[0].transform.localScale = Vector3.one;
            Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void GachaWeaponEX10()
    {
        StartCoroutine("gachaWeaEX10");
    }

    IEnumerator gachaWeaEX10()
    {
        gachaList.GetComponent<RectTransform>().position = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
            userData.weaponExes.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.weaponImg;
            content[0].transform.parent = gachaList.transform;
            content[0].transform.localScale = Vector3.one;
            Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
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
