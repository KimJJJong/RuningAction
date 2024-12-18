using UnityEngine;

public enum EEquipment
{
    Destroyer,      // ��ֹ� �ı��� ���� ȹ��
    Gold,        // �߰� Coin || EventStage Ȯ�� UP 
    LuckyGuy,
    NoPain,
    None,
}
public enum ERank
{
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
            case EEquipment.Gold:
                Gold(erank);
                break;
            case EEquipment.NoPain:
                NoPain(erank);
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
            case ERank.Normal:
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
    void Gold(ERank rank)
    {
        switch (rank)
        {
            case ERank.Normal:
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

    void NoPain(ERank rank)
    {
        switch (rank)
        {
            case ERank.Normal:
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
            case ERank.Normal:
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
