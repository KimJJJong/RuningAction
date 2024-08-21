using UnityEngine;

public enum EEquipment
{
    None,
    Destroyer,      // Àå¾Ö¹° ÆÄ±«½Ã Á¡¼ö È¹µæ
    Grubber,        // Ãß°¡ Coin || EventStage È®·ü UP 
}
public enum ERank
{
    None,
    Normal,
    Rare,
    Epic,
    Legendary,
    Mythic,
}

public class Equipment : MonoBehaviour
{
    public EEquipment equip;
    public ERank erank;

    private void Start()
    {
        setEquipment(equip, erank);
    }

    public void setEquipment(EEquipment equip, ERank erank)
    {
        switch (equip)
        {
            case EEquipment.None:
                break;
            case EEquipment.Destroyer:
                Destroyer(erank);
                break;
            case EEquipment.Grubber:
                Grubber(erank);
                break;
        }
    }


    void Destroyer(ERank rank)
    {
        switch (rank)
        {
            case ERank.None:
                GameManager.Instance.score.setIncreasObsRate(20);
                break;
            case ERank.Rare:
                GameManager.Instance.score.setIncreasObsRate(30);
                break;
            case ERank.Epic:
                GameManager.Instance.score.setIncreasObsRate(40);
                break;
            case ERank.Legendary:
                GameManager.Instance.score.setIncreasObsRate(50);
                break;
            case ERank.Mythic:
                GameManager.Instance.score.setIncreasObsRate(60);
                break;
        }
    }
    void Grubber(ERank rank)
    {
        switch (rank)
        {
            case ERank.None:
                GameManager.Instance.score.setIncreasCoinRate(0);
                break;
            case ERank.Rare:
                GameManager.Instance.score.setIncreasCoinRate(10);
                break;
            case ERank.Epic:
                GameManager.Instance.score.setIncreasCoinRate(15);
                break;
            case ERank.Legendary:
                GameManager.Instance.score.setIncreasCoinRate(20);
                break;
            case ERank.Mythic:
                GameManager.Instance.score.setIncreasCoinRate(25);
                break;
        }
    }




}
