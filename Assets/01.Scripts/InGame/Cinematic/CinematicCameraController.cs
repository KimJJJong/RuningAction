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
    private bool is_cine_ended = false;

    void Start()
    {                
        timer = 0;
        //main_camera.SetActive(false);

        //main_camera.GetComponent<CameraFollowPlayer>().distance;

        updateCine();
    }

    void Update()
    {        
        //For simplicity, CineManager only checks duration for now.
        //If checks if cine unit is really ended with following condition function, it would be perfect: if(cur_cine.IsCineEnded())
        if(!is_cine_ended && cur_cine.IsCineEnded())
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
            cur_duration = cur_cine.getDuration();
            cur_cine.startCine();
        }
        else
        {
            //End all cinematics
            is_cine_ended = true;
            main_camera.SetActive(true);   
        }
    }
}

