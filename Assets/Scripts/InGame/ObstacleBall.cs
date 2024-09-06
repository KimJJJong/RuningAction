using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObstacleBall : MonoBehaviour
{
    [SerializeField] float trigerDistance = 50;
    //float playerSpeed;
    Transform target = null;
    
    float ballSpeed = 10f;

    private void Start()
    {
        target = GameManager.Instance.player.GetComponent<Transform>();
      //  playerSpeed = GameManager.Instance.playerController.runningSpeed;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < trigerDistance)
        {
            Vector3 dir = new Vector3(0,0,-1);
            transform.Translate(dir.normalized * (ballSpeed /*+ playerSpeed*/) *Time.deltaTime);
        }
    }

  



}
