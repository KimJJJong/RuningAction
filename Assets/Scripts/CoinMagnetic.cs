using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CoinMagnetic : MonoBehaviour
{

    void Update()
    {
        Transform Target = GameManager.Instance.player.transform;
        if (GameManager.Instance.player.GetComponent<PlayerController>().selectEWeapon == 2)
        {
            if( Vector3.Distance(transform.position, Target.position) < 5)
            transform.position = Vector3.MoveTowards(transform.position, Target.position, 30 * Time.deltaTime);

        }
    }
}
