using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefab : MonoBehaviour
{
    public int prefab_id;
    public float prefab_length;
    public Vector3 prefab_size;

    private bool in_use = false;

    private void Awake() {
        prefab_size = calculatePrefabLength();
    }

    private void Start()
    {
        
    }

    public int getID()
    {
        return prefab_id;
    }

    public float getLength()
    {
        return prefab_length;
    }

    Vector3 calculatePrefabLength()
    { 
        Bounds bounds = new Bounds();
        foreach(Renderer render in this.gameObject.transform.GetComponentsInChildren<Renderer>()){
            if(render.gameObject.tag == "Ground")
                bounds.Encapsulate(render.bounds);
        }
        return bounds.size;
    }
}