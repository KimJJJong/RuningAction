using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReadyMenuManager : MonoBehaviour
{
    public Image charImage;
    public Image weaponBetImage;
    public Image weaponGloveImage;
    public Image weaponExImage;

    private UserData userData;
    public string userID = "001";

    public GameObject characterSelectList;
    public GameObject weaponBetSelectList;
    public GameObject weaponGloveSelectList;
    public GameObject weaponExSelectList;
    public GameObject upgraderSelectList;

    public GameObject characterSelectButton;
    public GameObject weaponBetSelectButton;
    public GameObject weaponGloveSelectButton;
    public GameObject weaponExSelectButton;
    public GameObject upgradeImg;

    private void Awake()
    {

    }

    private void Start()
    {
        userData = DataManager.instance.userData;

        for (int i = 0; i < userData.characters.Count; i++)
        {
            var chr = userData.characters[i];
            var btn = Instantiate(characterSelectButton);

            btn.GetComponent<Button>().image.sprite = chr.characterImg;
            btn.GetComponent<ReadyItem>().SetStar(chr);

            int index = i; // 람다에서 참조하기 위한 지역 변수로 저장 클로저?
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                UpdateCharImage(btn.GetComponent<Button>());
                DataManager.instance.userData.selectedCharacter = index;
                Debug.Log("Selected Character Index: " + index);
            });

            btn.transform.parent = characterSelectList.transform;
            btn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }

       for( int i=0; i<userData.weapons.Count;i++)
        {
            var weapon = userData.weapons[i];
            switch (weapon.weaponClass)
            {
                case WeaponData.WeaponClass.Bet:
                    var btn_bet = Instantiate(weaponBetSelectButton);
                    btn_bet.GetComponent<Button>().image.sprite= weapon.weaponImg;
                    btn_bet.GetComponent<ReadyItem>().SetStar(weapon);

                    int index_bet = i;
                    btn_bet.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        UpdateWeaponBetImage(btn_bet.GetComponent<Button>());
                        DataManager.instance.userData.selectedWeaponBat = index_bet;
                        Debug.Log("Selected WeaponBat Index: " + index_bet);
                    });
                        btn_bet.transform.parent = weaponBetSelectList.transform;
                        btn_bet.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                break;
                case WeaponData.WeaponClass.Glove:
                    var btn_glove = Instantiate(weaponGloveSelectButton);
                    btn_glove.GetComponent<Button>().image.sprite = weapon.weaponImg;
                    btn_glove.GetComponent<ReadyItem>().SetStar(weapon);

                    int index_glove = i;
                    btn_glove.GetComponent<Button>().onClick.AddListener(() =>
                    { 
                        UpdateWeaponGloveImage(btn_glove.GetComponent<Button>());
                        DataManager.instance.userData.selectedWeaponGlove = index_glove;
                        Debug.Log("Selected WeaponGlove Index: " + index_glove);
                    });
                        btn_glove.transform.parent = weaponGloveSelectList.transform;
                        btn_glove.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    break;

            }
        }
        /*
           foreach (var weapon in userData.weapons)
          {
              switch (weapon.weaponClass)
              {
                  case WeaponData.WeaponClass.Bet:
                      var btn_bet = Instantiate(weaponBetSelectButton);
                      btn_bet.GetComponent<Button>().image.sprite = weapon.weaponImg;
                      btn_bet.GetComponent<ReadyItem>().SetStar(weapon);
                      btn_bet.GetComponent<Button>().onClick.AddListener(() => UpdateWeaponBetImage(btn_bet.GetComponent<Button>()));
                      btn_bet.transform.parent = weaponBetSelectList.transform;
                      btn_bet.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                      break;
                  case WeaponData.WeaponClass.Glove:
                      var btn_glove = Instantiate(weaponGloveSelectButton);
                      btn_glove.GetComponent<Button>().image.sprite = weapon.weaponImg;
                      btn_glove.GetComponent<ReadyItem>().SetStar(weapon);
                      btn_glove.GetComponent<Button>().onClick.AddListener(() => UpdateWeaponGloveImage(btn_glove.GetComponent<Button>()));
                      btn_glove.transform.parent = weaponGloveSelectList.transform;
                      btn_glove.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                      break;
              }
          }
         */

       for(int i = 0; i < userData.weaponExes.Count; i++)
        {
            var weaponEx = userData.weaponExes[i];
            var btn = Instantiate(weaponExSelectButton);

            btn.GetComponent<Button>().image.sprite= weaponEx.weaponImg;
            btn.GetComponent <ReadyItem>().SetStar(weaponEx);

            int index_weapon = i;
            btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UpdateWeaponExImage(btn.GetComponent<Button>());
                    DataManager.instance.userData.selectedWeaponExes = index_weapon;
                    Debug.Log("Selected WeaponsExe Index: " + index_weapon);
            });
            btn.transform.parent = weaponExSelectList.transform;
            btn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        /*
         foreach (var weaponEx in userData.weaponExes)
        {
            var btn = Instantiate(weaponExSelectButton);
            btn.GetComponent<Button>().image.sprite = weaponEx.weaponImg;
            btn.GetComponent<ReadyItem>().SetStar(weaponEx);
            btn.GetComponent<Button>().onClick.AddListener(() => UpdateWeaponExImage(btn.GetComponent<Button>()));
            btn.transform.parent = weaponExSelectList.transform;
            btn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
         */
        foreach (var upgrade in userData.upgrades)
        {
            var img = Instantiate(upgradeImg);
            img.GetComponent<Image>().sprite = upgrade.upgradeImg;
            img.transform.parent = upgraderSelectList.transform;
            img.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

    }

    public void UpdateCharImage(Button btn)
    {
        charImage.sprite = btn.image.sprite;
        charImage.color = btn.image.color;
    }

    public void UpdateWeaponBetImage(Button btn)
    {
        weaponBetImage.sprite = btn.image.sprite;
        weaponBetImage.color = btn.image.color;
    }
    public void UpdateWeaponGloveImage(Button btn)
    {
        weaponGloveImage.sprite = btn.image.sprite;
        weaponGloveImage.color = btn.image.color;
    }
    public void UpdateWeaponExImage(Button btn)
    {
        weaponExImage.sprite = btn.image.sprite;
        weaponExImage.color = btn.image.color;
    }
}
