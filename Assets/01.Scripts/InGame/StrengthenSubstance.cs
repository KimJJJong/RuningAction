using UnityEngine;
using UnityEngine.UI;

public enum EEnforceIteam
{
    Hp,

}
/// <summary>
/// ����
/// </summary>
public class StrengthenSubstance : MonoBehaviour
{
    [SerializeField] int _hpLv = 0;
    [SerializeField] int _goldStageLv = 0;
    [SerializeField] int _rushLv = 0;
    [SerializeField] int _magneticLv = 0;

    public int RushLv => _rushLv;
    public int MagneticLv => _magneticLv;
    public int GoldStageLv => _goldStageLv;


    private void Start()
    {
        setState();
    }
    void setState()
    {
        SetHpState(_hpLv);
        setGoldStageState(_goldStageLv);
    }

    public void SetHpState(int hpLv)
    {
        GameManager.Instance.player.GetComponent<HpController>().SetHp(hpLv * 10);
        GameManager.Instance.player.GetComponent<HpController>()
            .HpBar.GetComponent<RectTransform>().sizeDelta
            = new Vector2(270 + (hpLv * 10), 30);

    }
    public void SetCoinState(int coinLv)
    {

    }

    public void SetRushState(int rushLv)
    {

    }
    //
    void setGoldStageState(int goldStageLv)
    {

    }

    void setRushState(int rushLv)
    {

    }

    void setMagneticState(int magneticLv)
    {

    }


}
