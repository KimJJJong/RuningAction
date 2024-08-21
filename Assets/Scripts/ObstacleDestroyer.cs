using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] string obstacle;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacle))
        {

            GameManager.Instance.player.GetComponent<Weapon>().GageSlider.value++;
            Destroy(gameObject);

        }
    }
}
