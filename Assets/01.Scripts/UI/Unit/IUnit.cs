using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public void UpgradeUnit();
    public IItemData GetData();
}
