using UnityEngine;
using UnityEngine.UI;

public enum EWeapon
{
    Bat,
    Ball,
    Magnetic,
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
    }

    private void Update()
    {
        updateGage();

    }

    public void Triger(EWeapon type)
    {

        switch (type)
        {
            case EWeapon.Bat:
                Batting();
                break;
            case EWeapon.Ball:
                throughtBall();
                break;
            case EWeapon.Magnetic:
                break;


        }
    }



    void Batting()
    {
        hitBox.SetActive(true);
        hitBox.transform.position = gameObject.transform.position + new Vector3(0, 1f, 2f);
    }

    void throughtBall()
    {
        Vector3 pos = gameObject.transform.position;
        GameObject _ball = Instantiate(Ball, new Vector3(pos.x, pos.y + 1, pos.z + 1), Quaternion.identity);
    }

    void updateGage()
    {
        if (GageSlider.value >= GageSlider.maxValue)
        {
            GameManager.Instance.player.GetComponent<PlayerController>().Invincibility();

        }
    }
}
