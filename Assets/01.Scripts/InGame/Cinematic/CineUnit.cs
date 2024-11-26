using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineUnit : MonoBehaviour
{ 
    public GameObject virtual_camera;


    [SerializeField]
    private float duration;
    public float getDuration() { return duration; }
        

    void Start()
    {
        virtual_camera.SetActive(false);
    }

    //Can accept bool to decide if v_cam will be turned on, but it cannot be mixed conceptually
    public void startCine()
    {        
        virtual_camera.gameObject.SetActive(true);    
    }

    public void endCine()
    {        
        virtual_camera.gameObject.SetActive(false);      
    }

    //For simplicity, CineManager only checks duration for checking if cine unit is end, but using this can synchronize if cine unit is relaly ended.
    public bool IsCineEnded()
    {
        //put actual condition if cine is end and return it on CineManager's update loop.
        return true;
    }


    

}
