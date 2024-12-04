using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    public ObstacleManager.ObstacleType obstacle_type;
    
    void Start()
    {
        SetCollisionBox();
    }

    public void SetCollisionBox()
    {
        BoxCollider box_collider = this.AddComponent<BoxCollider>();

        box_collider.isTrigger = true;

        switch(obstacle_type)
        {
            case ObstacleManager.ObstacleType.jump_obstacle_short:
                box_collider.size = new Vector3(2, 1, 1);
                box_collider.center = new Vector3(0, 0.5f, 0);
                break;

            case ObstacleManager.ObstacleType.jump_obstacle_long:
                box_collider.size = new Vector3(2, 1, 2);
                box_collider.center = new Vector3(0, 0.5f, 0);
                break;

            case ObstacleManager.ObstacleType.slide_obstacle:
            break;
            case ObstacleManager.ObstacleType.block_obstacle:
            break;
            case ObstacleManager.ObstacleType.defender_obstacle:
            break;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ObstacleManager.instance != null)
                ObstacleManager.instance.HandleObstacleCollision(other.gameObject, this);
            else
                Debug.Log("Item: No item manager instance");
            

            Destroy(gameObject);
        }
    }
}

