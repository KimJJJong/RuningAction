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
    [HideInInspector] public int goldStageProbability;


    void Update()
    {

        if (groundsQueue.Count <= 10)
        {
            EnqueueRandomGround();
         
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

    private void EnqueueRandomGround()
    {
        if (Random.Range(0, 100) <= 10 + goldStageProbability)
        {
            groundsQueue.Enqueue(grounds[grounds.Length - 1]);
        }
        else
        {
            int selectedIndex = Random.Range(0, grounds.Length - 1);

            if (groundsQueue.Count > 0 && grounds[selectedIndex] == groundsQueue.Peek())
            {
                selectedIndex = Random.Range(0, grounds.Length - 1); // Choose next object in the array

            }

            groundsQueue.Enqueue(grounds[selectedIndex]);
        }
    }

}