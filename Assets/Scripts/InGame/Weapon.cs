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
    /// 상호작용 제어 메소드
    /// </summary>
    /// <param name="type">캐릭터 직업의 타입</param>
    /// <param name="chRank">캐릭터의 레어도</param>
    /// <param name="ch">상호작용 Obj소환</param>
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
            if(GameManager.Instance.playerController.eCh == ECharacter.Bat)
                GameManager.Instance.player.GetComponent<PlayerController>().Invincibility(false);
            if (GameManager.Instance.playerController.eCh == ECharacter.Glove)
                Debug.Log("아직 구현안함요");
        }
    }


    void InitWeapon()
    {
        ECharacter type = GameManager.Instance.playerController.eCh;
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
    public void GageUpdate()    // 오브젝트가 파괴될 때 마다 update
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
                _gageSize = 6.7f;   // 15번
                break;
            case ERank.Rare:
                _gageSize = 7.7f;   // 13번
                break;
            case ERank.Epic:
                _gageSize = 9.1f;   // 11번
                break;
            case ERank.Legendary:
                _gageSize = 11.12f; // 9번
                break;
            case ERank.Mythic:
                _gageSize = 14.3f;  // 7번
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
                _gageSize = 11.2f;   // 9번
                break;
            case ERank.Rare:
                _gageSize = 12.5f;   // 8번
                break;
            case ERank.Epic:
                _gageSize = 14.3f;   // 7번
                break;
            case ERank.Legendary:
                _gageSize = 16.7f; // 6번
                break;
            case ERank.Mythic:
                _gageSize = 20f;  // 5번
                break;
            default:
                _gageSize = 0;
                break;

        }
    }
}
