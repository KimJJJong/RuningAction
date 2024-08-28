using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    static public int isSpawning = 0;
    public GameObject[] grounds;

    Queue<GameObject> groundsQueue = new Queue<GameObject>();
    int distance = 78;
    int var;


    void Update()
    {

        if (groundsQueue.Count <= 10)
        {
            if (Random.Range(0, 10) == 1)
            {
                groundsQueue.Enqueue(grounds[grounds.Length - 1]);
                //Debug.Log($"{grounds[grounds.Length-1]}");
            }
            else
            {
                var = Random.Range(0, grounds.Length - 1);
                groundsQueue.Enqueue(grounds[var]);
            }
        }
        if (isSpawning < 5)
        {
            isSpawning++;
            StartCoroutine(SpawnGround());
        }

    }
    IEnumerator SpawnGround()
    {
        Instantiate(groundsQueue.Dequeue(), new Vector3(-0.4133758f, -39.25718f, distance), Quaternion.identity);
        distance += 39;
        yield return new WaitForSeconds(0.5f);
    }
    
}