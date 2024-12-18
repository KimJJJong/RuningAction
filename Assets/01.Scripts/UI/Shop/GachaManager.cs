using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GachaSet.SetActive(true);
        var result = Instantiate(characters[Random.Range(0, characters.Count)]);
        userData.characters.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.characterImg;
        content[0].transform.parent = gachaList.transform;
        content[0].transform.localScale = Vector3.one;
        switch (result.rarelity)
        {
            case CharacterData.Rarelity.Normal:
                content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                break;

            case CharacterData.Rarelity.Rare:
                content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                break;

            case CharacterData.Rarelity.Epic:
                content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                break;

            case CharacterData.Rarelity.Legendary:
                content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                break;

            case CharacterData.Rarelity.Mythic:
                content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                break;
        }
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
    }

    public void GachaWeapon()
    {
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
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
        switch (result.rarelity)
        {
            case WeaponData.Rarelity.Normal:
                content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                break;

            case WeaponData.Rarelity.Rare:
                content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                break;

            case WeaponData.Rarelity.Epic:
                content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                break;

            case WeaponData.Rarelity.Legendary:
                content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                break;

            case WeaponData.Rarelity.Mythic:
                content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                break;
        }
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);

    }

    public void GachaWeaponEX()
    {
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GachaSet.SetActive(true);
        var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
        userData.weaponExes.Add(result);
        var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
        content[1].sprite = result.weaponImg;
        content[0].transform.parent = gachaList.transform;
        content[0].transform.localScale = Vector3.one;
        switch (result.rarelity)
        {
            case WeaponEXData.Rarelity.Normal:
                content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                break;

            case WeaponEXData.Rarelity.Rare:
                content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                break;

            case WeaponEXData.Rarelity.Epic:
                content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                break;

            case WeaponEXData.Rarelity.Legendary:
                content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                break;

            case WeaponEXData.Rarelity.Mythic:
                content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                break;
        }
        Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
    }

    public void GachaCharacter10()
    {
        StartCoroutine("gachaChar10");
    }

    IEnumerator gachaChar10()
    {
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(characters[Random.Range(0, characters.Count)]);
            userData.characters.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.characterImg;
            content[0].transform.parent = gachaList.transform;
            content[0].transform.localScale = Vector3.one;
            switch (result.rarelity)
            {
                case CharacterData.Rarelity.Normal:
                    content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                    break;

                case CharacterData.Rarelity.Rare:
                    content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                    break;

                case CharacterData.Rarelity.Epic:
                    content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                    break;

                case CharacterData.Rarelity.Legendary:
                    content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                    break;

                case CharacterData.Rarelity.Mythic:
                    content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                    break;
            }
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
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
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
            switch (result.rarelity)
            {
                case WeaponData.Rarelity.Normal:
                    content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                    break;

                case WeaponData.Rarelity.Rare:
                    content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                    break;

                case WeaponData.Rarelity.Epic:
                    content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                    break;

                case WeaponData.Rarelity.Legendary:
                    content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                    break;

                case WeaponData.Rarelity.Mythic:
                    content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                    break;
            }
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
        gachaList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GachaSet.SetActive(true);
            var result = Instantiate(weaponExes[Random.Range(0, weaponExes.Count)]);
            userData.weaponExes.Add(result);
            var content = Instantiate(gachaContent).GetComponentsInChildren<Image>();
            content[1].sprite = result.weaponImg;
            content[0].transform.parent = gachaList.transform;
            content[0].transform.localScale = Vector3.one;
            switch (result.rarelity)
            {
                case WeaponEXData.Rarelity.Normal:
                    content[0].transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f, 1, 1);
                    break;

                case WeaponEXData.Rarelity.Rare:
                    content[0].transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f, 1, 1);
                    break;

                case WeaponEXData.Rarelity.Epic:
                    content[0].transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1);
                    break;

                case WeaponEXData.Rarelity.Legendary:
                    content[0].transform.DOPunchScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f, 1, 1);
                    break;

                case WeaponEXData.Rarelity.Mythic:
                    content[0].transform.DOPunchScale(new Vector3(0.7f, 0.7f, 0.7f), 0.5f, 1, 1);
                    break;
            }
            Instantiate(gachaEffects[(int)result.rarelity], content[0].transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void EndGacha()
    {
        SortItems();
        GachaSet.SetActive(false);
        foreach (var obj in gachaList.GetComponentsInChildren<Image>())
        {
            Destroy(obj.gameObject);
        }
    }

    public void SortItems()
    {
        userData.characters.Sort((x, y) => y.GetID().CompareTo(x.GetID()));
        userData.bets.Sort((x, y) => y.GetID().CompareTo(x.GetID()));
        userData.gloves.Sort((x, y) => y.GetID().CompareTo(x.GetID()));
        userData.weaponExes.Sort((x, y) => y.GetID().CompareTo(x.GetID()));
    }
}
