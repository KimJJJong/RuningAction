using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] grounds;
    int distance = 78;
  //  bool isSpawning = false;
    static public int isSpawning = 0;
    int var;


    void Start()
    {
    }

    void Update()
    {
        /*if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnGround());
        }*/
        if (isSpawning < 5)
        {
            isSpawning ++;
            StartCoroutine(SpawnGround());
        }
    }
    IEnumerator SpawnGround()
    {
        var = Random.Range(0, grounds.Length);
        Instantiate(grounds[var], new Vector3(-0.4133758f, -39.25718f, distance), Quaternion.identity);
        distance += 39;
        yield return new WaitForSeconds(0.5f);
    }


}