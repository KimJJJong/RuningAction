using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CinematicCameraController : MonoBehaviour
{        
    public GameObject main_camera;

    public List<CineUnit> cines;
    
    private CineUnit cur_cine;

    private float cur_duration;
    private float timer;
    private bool is_whole_cines_ended = false;

    void Start()
    {                
        updateCine();
    }

    void Update()
    {        
        if(!is_whole_cines_ended && cur_cine.IsCineEnded())
        {                 
            cines.RemoveAt(0);
            updateCine();
            timer = 0;
        }

        //timer += Time.deltaTime;                
    }

    //checking all cines are done and if not, setting new cur_cine
    void updateCine()
    {     
        if(cines.Count > 0)
        {
            cur_cine = cines[0];            
            cur_cine.startCine();
        }
        else
        {
            //End all cinematics
            is_whole_cines_ended = true;
            main_camera.SetActive(true);   
        }
    }
}

