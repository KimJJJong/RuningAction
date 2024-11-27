using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



//According to type of CineUnit, it can diverged into several types of CineUnit with parent class that has commonalities for every type of CineUnit.
//However, there is less possibility of using other type of Cine Unit other than dolly cart, so didn't make other type or parent class for Cine unit.
public class CineUnit : MonoBehaviour
{ 
    public CinemachineVirtualCamera virtual_camera;

    private bool is_done = false;

    public CinemachineDollyCart dollyCart; 
    public CinemachineSmoothPath path;
 
    private void Update()
    {
        if(!is_done)
        //Only for dolly cart Cine Unit
            if (dollyCart.m_Position >= path.PathLength)
                endCine();
    }

    //Can accept bool to decide if v_cam will be turned on, but it cannot be mixed conceptually
    public void startCine()
    {        
        virtual_camera.enabled = true;
    }

    public void endCine()
    {        
        virtual_camera.enabled = false;
        is_done = true;
    }
    
    public bool IsCineEnded()
    {        
        return is_done;                
    }


    

}
