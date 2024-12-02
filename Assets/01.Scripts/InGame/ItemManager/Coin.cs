using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    [SerializeField]
    private CoinManager.CoinType coin_type;

    private void Awake()
    {
        id = "Coin";
        is_storable = false;
    }

    public override void use()
    {
        CoinManager.instance.obtainCoin(coin_type);
    }

}



