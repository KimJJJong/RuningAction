using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MapPrefab : MonoBehaviour
{
    public int prefab_id;
    public float prefab_length;
    public Vector3 prefab_size;

    private bool in_use = false;

    public List<Transform> objects_to_reset;

    //Only storage, must be private
    private List<Vector3> objects_to_reset_org_pos = new List<Vector3>();
    private List<Quaternion> objects_to_reset_org_rot = new List<Quaternion>();    

    [HideInInspector]
    public IObjectPool<GameObject> pool { get; set; }

    private void Awake()
    {
        prefab_size = calculatePrefabLength();
    }
    

    private void OnEnable()
    {
        //When prefab is turned on, save org position for moving objects in prefab.
        //If the prefab is turned on again, retrieve that info to make the prefab as new.
        if(objects_to_reset_org_pos.Count == 0)
        {
            for(int i = 0; i < objects_to_reset.Count; i++)    
            {
                objects_to_reset_org_pos.Add(objects_to_reset[i].position);
                objects_to_reset_org_rot.Add(objects_to_reset[i].rotation);                
            }                    
        }
        else
        {
            for(int i = 0; i < objects_to_reset_org_pos.Count; i++)
            {
                objects_to_reset[i].position = objects_to_reset_org_pos[i];
                objects_to_reset[i].rotation = objects_to_reset_org_rot[i];                        
            }

        }        
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
        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        foreach (Renderer render in this.gameObject.transform.GetComponentsInChildren<Renderer>())
        {
            if (render.gameObject.tag == "Ground")
            {
                bounds.Encapsulate(render.bounds);
            }
        }
        return bounds.size;
    }

    public void ReleaseObject()
    {
        pool.Release(gameObject);
    }
}
