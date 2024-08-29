using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyMenuManager : MonoBehaviour
{
    public Image charImage;
    public Image weaponImage;

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
