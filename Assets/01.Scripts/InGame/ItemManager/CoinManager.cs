using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using DarkTonic.MasterAudio;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance { get; private set; }

    private Dictionary<CoinType, int> coin_value_dic = new Dictionary<CoinType, int>();

    private int total_coin = 0;
    private GameUIManager gameUIManager;

    public enum CoinType
    {        
        normal,
        special
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //TODO: Get a coin info from server
        coin_value_dic[CoinType.normal] = 10;
        coin_value_dic[CoinType.special] = 30;

        gameUIManager = GameUIManager.instance;
    }

    public void obtainCoin(CoinType coin_type)
    {
        int value = coin_value_dic[coin_type];
        
        //TODO: apply user ability to have a bonus coin
        //value *= userAbility.bonus_coin;

        total_coin += value;
        gameUIManager.UpdateCoinText(total_coin);
        MasterAudio.PlaySound("coin");

    }
 
}
