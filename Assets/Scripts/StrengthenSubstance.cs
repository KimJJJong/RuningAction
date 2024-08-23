using UnityEngine;
using UnityEngine.UI;

public enum EEnforceIteam
{
    Hp,

}

public class StrengthenSubstance : MonoBehaviour
{
    [SerializeField] int hpLv = 0;

    private void Start()
    {
        setState();
    }
    void setState()
    {
        GameManager.Instance.player.GetComponent<HpController>().setHp(hpLv * 10);
        GameManager.Instance.player.GetComponent<HpController>()
            .HpBar.GetComponent<RectTransform>().sizeDelta
            = new Vector2(270 + (hpLv * 10), 30);
    }
}
