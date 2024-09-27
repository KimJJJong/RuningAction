using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    UserData userData;

    public TMP_Text userName;
    public TMP_Text userGold;
    public TMP_Text userDia;

    public List<Button> menuButtons = new List<Button>();
    public List<GameObject> menuTexts = new List<GameObject>();
    public List<Image> menuImages = new List<Image>();
    public List<Sprite> menuFrames = new List<Sprite>();

    private void Start()
    {
        userData = DataManager.instance.userData;
        UpdateUI();
    }

    public void UpdateUI()
    {
        userName.text = userData.userName;
        userGold.text = userData.money.ToString();
        //userDia.text = userData.dia.ToString();
    }

    public void ChangeMenu(int num)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == num)
            {
                menuButtons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(290, 120);
                menuTexts[i].SetActive(true);
                menuImages[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 40, 0);
                menuImages[i].GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
                menuButtons[i].image.sprite = menuFrames[0];
            }
            else
            {
                menuButtons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
                menuTexts[i].SetActive(false);
                menuImages[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 15, 0);
                menuImages[i].GetComponent<RectTransform>().localScale = Vector3.one;
                menuButtons[i].image.sprite = menuFrames[1];
            }
        }
    }

}
