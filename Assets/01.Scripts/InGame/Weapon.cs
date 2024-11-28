using UnityEngine;
using UnityEngine.UI;

public enum ECharacter
{
    Bat,
    Glove,
    Magnetic,
}

public class Weapon : MonoBehaviour
{
    public Slider GageSlider;
    public GameObject Ball;

    [Space(10)]
    // public int batLv = 0;
    // public int gloveLv = 0;
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

    /// <summary>
    /// ��ȣ�ۿ� ���� �޼ҵ�
    /// </summary>
    /// <param name="type">ĳ���� ������ Ÿ��</param>
    /// <param name="chRank">ĳ������ ���</param>
    /// <param name="ch">��ȣ�ۿ� Obj��ȯ</param>
    public void Triger(ECharacter type, ERank chRank, ECharacter ch)
    {
        if (ch == ECharacter.Bat)
            Throw();
        if (ch == ECharacter.Glove)
            Batting();
        switch (type)
        {
            case ECharacter.Bat:
                BatLv(chRank);
                GageSlider.maxValue = 100;
                break;
            case ECharacter.Glove:
                GloveLv(chRank);
                GageSlider.maxValue = 100;
                break;
            case ECharacter.Magnetic:
                BeMagnetic();
                break;
        }
    }

    void BeMagnetic() { }

    void Batting()
    {
        hitBox.SetActive(true);
        hitBox.transform.position = gameObject.transform.position + new Vector3(0, 1f, 2f);
    }

    void Throw()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject _ball = Instantiate(
            Ball,
            new Vector3(pos.x, pos.y + 1, pos.z + 1),
            Quaternion.identity
        );
    }

    void UpdateGage()
    {
        if (GageSlider.value >= GageSlider.maxValue)
        {
            PlayerController playerController =
                GameManager.Instance.playController.GetCurrentController();

            if (playerController.eCh == ECharacter.Bat)
                playerController.Invincibility(false);
            if (playerController.eCh == ECharacter.Glove)
                Debug.Log("���� �������Կ�");
        }
    }

    void InitWeapon()
    {
        PlayerController playerController =
            GameManager.Instance.playController.GetCurrentController();

        ECharacter type = playerController.eCh;
        switch (type)
        {
            case ECharacter.Bat:
                break;
            case ECharacter.Glove:
                break;
            case ECharacter.Magnetic:
                GageSlider.gameObject.SetActive(false);
                break;
        }
    }

    public void GageUpdate() // ������Ʈ�� �ı��� �� ���� update
    {
        GageSlider.value += _gageSize;
    }

    public void DecreaseGage(float num)
    {
        GageSlider.value -= num;
    }

    public void BatLv(ERank rank)
    {
        switch (rank)
        {
            case ERank.Normal:
                _gageSize = 6.7f; // 15��
                break;
            case ERank.Rare:
                _gageSize = 7.7f; // 13��
                break;
            case ERank.Epic:
                _gageSize = 9.1f; // 11��
                break;
            case ERank.Legendary:
                _gageSize = 11.12f; // 9��
                break;
            case ERank.Mythic:
                _gageSize = 14.3f; // 7��
                break;
            default:
                _gageSize = 0;
                break;
        }
    }

    public void GloveLv(ERank rank)
    {
        switch (rank)
        {
            case ERank.Normal:
                _gageSize = 11.2f; // 9��
                break;
            case ERank.Rare:
                _gageSize = 12.5f; // 8��
                break;
            case ERank.Epic:
                _gageSize = 14.3f; // 7��
                break;
            case ERank.Legendary:
                _gageSize = 16.7f; // 6��
                break;
            case ERank.Mythic:
                _gageSize = 20f; // 5��
                break;
            default:
                _gageSize = 0;
                break;
        }
    }
}
