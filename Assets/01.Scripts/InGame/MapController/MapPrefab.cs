using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefab : MonoBehaviour
{
    int prefab_id;
    float prefab_length;
    GameObject prefab;

    private void Start()
    {
        prefab = GetComponent<GameObject>();    
    }

    public int getID()
    {
        return prefab_id;
    }

    public float getLength()
    {
        return prefab_length;
    }

    void calculatePrefabLength(Vector3 orientation)
    { 
        //orientation에 따라 prefab Length계산
    }

    

}