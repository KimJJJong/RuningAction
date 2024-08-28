using UnityEngine;
using UnityEngine.UI;

public enum EWeapon
{
    Bat = 0,
    Ball = 1,
    Magnetic = 2,
}

public class Weapon : MonoBehaviour
{
    public Slider GageSlider;
    public GameObject Ball;

    GameObject hitBox;
    private void Start()
    {
        hitBox = GameObject.Find("HitBox");
        hitBox.SetActive(false);
        InitWeapon();
    }

    private void Update()
    {
        UpdateGage();

    }

    public void Triger(EWeapon type)
    {

        switch (type)
        {
            case EWeapon.Bat:
                Batting();
                GageSlider.maxValue = 10;
                break;
            case EWeapon.Ball:
                GageSlider.maxValue = 5;
                ThroughtBall();
                break;
            case EWeapon.Magnetic:
                beMagnetic();
                break;


        }
    }


    void beMagnetic()
    {

    }

    void Batting()
    {
        hitBox.SetActive(true);
        hitBox.transform.position = gameObject.transform.position + new Vector3(0, 1f, 2f);
    }

    void ThroughtBall()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject _ball = Instantiate(Ball, new Vector3(pos.x, pos.y + 1, pos.z + 1), Quaternion.identity);
    }

    void UpdateGage()
    {
        if (GageSlider.value >= GageSlider.maxValue)
        {
            GameManager.Instance.player.GetComponent<PlayerController>().Invincibility(false);

        }
    }


    void InitWeapon()
    {
        EWeapon type = GameManager.Instance.playerController.eWeapons;
        switch (type)
        {
            case EWeapon.Bat:
                break;
            case EWeapon.Ball:
                break;
            case EWeapon.Magnetic:
                GageSlider.gameObject.SetActive(false);
                break;

        }

    }
}
