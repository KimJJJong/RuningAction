using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemData
{
    public void SetStarRate();
    public int GetStarRate();

    public int GetReinforceMat();
    public int GetReinforceGold();

    public int GetID();
}
