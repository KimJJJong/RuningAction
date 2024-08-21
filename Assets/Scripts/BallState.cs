using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallState : MonoBehaviour
{
    float curSpeed;
    void Start()
    {
        curSpeed = GameManager.Instance.player.GetComponent<PlayerController>().runningSpeed;
        Destroy(gameObject, 300f * Time.deltaTime);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
                                         transform.position.y , 
                                         transform.position.z + (curSpeed + 10f) * Time.deltaTime);


        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

    }

}
