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

    [Space(10)]

    public int batLv = 0;
    public int ballLv = 0;
    private float _gageSize;

    public float GageSize
    {
        get => _gageSize;
        set => _gageSize = value;
    }

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

    public void Triger(EWeapon type, int weaponNumber)
    {
        if (weaponNumber == 0)
            Throw();
        if (weaponNumber == 1)
            Batting();
        switch (type)
        {
            case EWeapon.Bat:
                BatLv(batLv);
                GageSlider.maxValue = 100;
                break;
            case EWeapon.Ball:
                BallLv(ballLv);
                GageSlider.maxValue = 100;
                break;
            case EWeapon.Magnetic:
                BeMagnetic();
                break;


        }
    }


    void BeMagnetic()
    {

    }

    void Batting()
    {
        hitBox.SetActive(true);
        hitBox.transform.position = gameObject.transform.position + new Vector3(0, 1f, 2f);
    }

    void Throw()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject _ball = Instantiate(Ball, new Vector3(pos.x, pos.y + 1, pos.z + 1), Quaternion.identity);
    }

    void UpdateGage()
    {
        if (GageSlider.value >= GageSlider.maxValue)
        {
            if(GameManager.Instance.playerController.eWeapons == EWeapon.Bat)
                GameManager.Instance.player.GetComponent<PlayerController>().Invincibility(false);
            if (GameManager.Instance.playerController.eWeapons == EWeapon.Ball)
                Debug.Log("���� �������Կ�");
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
    public void GageUpdate()    // ������Ʈ�� �ı��� �� ���� update
    {
        GageSlider.value += _gageSize;
    }
    public void DecreaseGage(float num)
    {
        GageSlider.value -= num;
    }
    public void BatLv(int lv)
    {
        switch (lv)
        {
            case 1:
                _gageSize = 6.7f;   // 15��
                break;
            case 2:
                _gageSize = 7.7f;   // 13��
                break;
            case 3:
                _gageSize = 9.1f;   // 11��
                break;
            case 4:
                _gageSize = 11.12f; // 9��
                break;
            case 5:
                _gageSize = 14.3f;  // 7��
                break;
            default:
                _gageSize = 0;
                break;

        }
    }
    public void BallLv(int lv)
    {
        switch (lv)
        {
            case 1:
                _gageSize = 11.2f;   // 9��
                break;
            case 2:
                _gageSize = 12.5f;   // 8��
                break;
            case 3:
                _gageSize = 14.3f;   // 7��
                break;
            case 4:
                _gageSize = 16.7f; // 6��
                break;
            case 5:
                _gageSize = 20f;  // 5��
                break;
            default:
                _gageSize = 0;
                break;

        }
    }
}
