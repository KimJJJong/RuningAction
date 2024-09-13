using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUserData : MonoBehaviour
{
    PlayerController controller;
    StrengthenSubstance substance;
    Equipment equipment;

    void Start()
    {
        foreach (var obj in DataManager.instance.userData.upgrades)
            Debug.Log(obj.upgradeName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
