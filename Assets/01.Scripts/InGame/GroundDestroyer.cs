using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{

    /*
      private void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Player"))
         {
             GroundSpawner.isSpawning--;
             Destroy(gameObject);
         }
     }
     */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }

}
