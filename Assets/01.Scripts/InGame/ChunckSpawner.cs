using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunckSpawner : MonoBehaviour
{
    //TODO: Get prew chunks
    [SerializeField] GameObject[] _chunks01;
    [SerializeField] GameObject[] _chunks02;
    //[SerializeField] GameObject[] _chunks03;
    [SerializeField] private float _chunkLenght = 119f;      // May be 119f?
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnDistance = 299f;    // 299f ??
    [Header("Pool Config")]
    [SerializeField] private int _size;
    [HideInInspector] public int goldStageProbability;

   // public int stageLv;     // 아직 어떻코론 해야할지 모르것당ㅇㅇㅇㅇㅇ;


    private List<Queue<GameObject>> _chunksQueueList01 = new List<Queue<GameObject>>(); //poolList for randomizing
    private List<Queue<GameObject>> _chunksQueueList02 = new List<Queue<GameObject>>();
    private Vector3 _spawnPos = new Vector3(0f, -38f, 0f);

    private void Awake()
    {
        _spawnPos = new Vector3(-0.4f, -39.25718f, 170);
    }
    private void Start()
    {
        PoolChunks(_chunksQueueList01,_chunks01);
        PoolChunks(_chunksQueueList02, _chunks02);
    }
    private void Update()
    {
        if (Vector3.Distance(_playerTransform.position, _spawnPos) < _spawnDistance)
        {
          if(_spawnPos.z > 800f)
            {
            Debug.Log(_spawnPos);
                SpawnRandomChunk(_chunksQueueList02);
            }
           else
           SpawnRandomChunk(_chunksQueueList01);
        }
    }
    private void PoolChunks(List<Queue<GameObject>> chuncksQueueList, GameObject[] chuncks)
    {
        for (int i = 0; i < chuncks.Length; i++)
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int j = 0; j < _size; j++)
            {
                GameObject newObj = Instantiate(chuncks[i].gameObject);
                newObj.gameObject.SetActive(false);
                newPool.Enqueue(newObj);
            }
            chuncksQueueList.Add(newPool);
            
        }
    }


    private void SpawnRandomChunk(List<Queue<GameObject>> chuncksQueueList)
    {
        if (Random.Range(0, 100) <= 5 + goldStageProbability)
        {
            GameObject newChunk = _chunksQueueList01[chuncksQueueList.Count-1].Dequeue();
            newChunk.transform.position = _spawnPos;
            _spawnPos.z += _chunkLenght;
            newChunk.gameObject.SetActive(true);
            _chunksQueueList01[chuncksQueueList.Count-1].Enqueue(newChunk);
        }
        else
        {
            int randValue = Random.Range(0, chuncksQueueList.Count - 1);
        GameObject newChunk = chuncksQueueList[randValue].Dequeue();
        newChunk.transform.position = _spawnPos;
        _spawnPos.z += _chunkLenght;
        newChunk.gameObject.SetActive(true);
            chuncksQueueList[randValue].Enqueue(newChunk);
        }
    }
}
