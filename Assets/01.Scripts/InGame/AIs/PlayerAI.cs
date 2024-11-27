using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    private bool is_enabled;

    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        
        //TODO: Perfect Error Handling is needed
        if(!playerController)
            Debug.Log("PlayerAI: cannot find PlayerController in Parent");
    }

    //TODO: fix magic number, 5.0f
    public float rayDistance = 5.0f;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            //for debugginh
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            Debug.Log("Hit Object Tag: " + hit.collider.tag);

            if(hit.collider.tag == "ObstacleJump")
                playerController.SetState(EState.Up);
            else if(hit.collider.tag == "ObstacleSlide")
                playerController.SetState(EState.Down);
        }                
    }
}
