using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    private void Awake()
    {
        id = "Health Potion";
        is_storable = true;
    }

    public override void use()
    {
        Debug.Log($"{id} item used");
   
    }
}



