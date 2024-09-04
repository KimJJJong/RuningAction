using UnityEngine;

public enum EEquipment
{
    None,
    Destroyer,      // Àå¾Ö¹° ÆÄ±«½Ã Á¡¼ö È¹µæ
    Grubber,        // Ãß°¡ Coin || EventStage È®·ü UP 
    PowerArmor,
    LuckyGuy,
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
            case EEquipment.PowerArmor:
                PowerArmor(erank);
                break;
            case EEquipment.LuckyGuy:
                LuckyGuy(erank);
                break;
        }
    }


    void Destroyer(ERank rank)
    {
        switch (rank)
        {
            case ERank.None:
                GameManager.Instance.score.SetIncreasObsRate(20);
                break;
            case ERank.Rare:
                GameManager.Instance.score.SetIncreasObsRate(30);
                break;
            case ERank.Epic:
                GameManager.Instance.score.SetIncreasObsRate(40);
                break;
            case ERank.Legendary:
                GameManager.Instance.score.SetIncreasObsRate(50);
                break;
            case ERank.Mythic:
                GameManager.Instance.score.SetIncreasObsRate(60);
                break;
        }
    }
    void Grubber(ERank rank)
    {
        switch (rank)
        {
            case ERank.None:
                GameManager.Instance.score.SetIncreasCoinRate(0);
                break;
            case ERank.Rare:
                GameManager.Instance.score.SetIncreasCoinRate(10);
                break;
            case ERank.Epic:
                GameManager.Instance.score.SetIncreasCoinRate(15);
                break;
            case ERank.Legendary:
                GameManager.Instance.score.SetIncreasCoinRate(20);
                break;
            case ERank.Mythic:
                GameManager.Instance.score.SetIncreasCoinRate(25);
                break;
        }
    }

    void PowerArmor(ERank rank)
    {
        switch (rank)
        {
            case ERank.None:
                GameManager.Instance.collisions.chance = 0;
                break;
            case ERank.Rare:
                GameManager.Instance.collisions.chance = 0.15f;
                break;
            case ERank.Epic:
                GameManager.Instance.collisions.chance = 0.30f;
                break;
            case ERank.Legendary:
                GameManager.Instance.collisions.chance = 0.45f;
                break;
            case ERank.Mythic:
                GameManager.Instance.collisions.chance = 0.60f;
                break;
        }
    }

    void LuckyGuy(ERank rank)
    {
        ChunckSpawner goldSpawn = GameObject.Find("ChuckSpawner").GetComponent<ChunckSpawner>();
        switch (rank)
        {
            case ERank.None:
                goldSpawn.goldStageProbability=12;
                break;
            case ERank.Rare:
                goldSpawn.goldStageProbability = 14;
                break;
            case ERank.Epic:
                goldSpawn.goldStageProbability = 16; 
                break;
            case ERank.Legendary:
                goldSpawn.goldStageProbability = 18;
                break;
            case ERank.Mythic:
                goldSpawn.goldStageProbability = 20;
                break;
        }
    }
}
