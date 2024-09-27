using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemData
{
    public string GetName();
    public string GetDesc();

    public void SetStarRate();
    public int GetStarRate();

    public int GetReinforceMat();
    public int GetReinforceGold();

    public int GetUpgradeRate(int rate);

    public int GetID();

    public int GetRarelity();

    public Sprite GetImage();
}
