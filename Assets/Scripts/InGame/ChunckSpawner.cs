using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunckSpawner : MonoBehaviour
{
    //TODO: Get prew chunks
    [SerializeField] GameObject[] _chunks;
    [SerializeField] private float _chunkLenght = 40f;      // May be 78f?
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnDistance = 199.9f;    // 38f ??
    [Header("Pool Config")]
    [SerializeField] private int _size;
    [HideInInspector] public int goldStageProbability;


    private List<Queue<GameObject>> _chunksQueueList = new List<Queue<GameObject>>(); //poolList for randomizing
    private Vector3 _spawnPos = new Vector3(0f, -38f, 0f);

    private void Awake()
    {
        _spawnPos = new Vector3(-0.4f, -39.25718f, 78f);
    }
    private void Start()
    {
        PoolChunks();
    }
    private void Update()
    {
        if (Vector3.Distance(_playerTransform.position, _spawnPos) < _spawnDistance)
        {
            SpawnRandomChunk();
        }
    }
    private void PoolChunks()
    {
        for (int i = 0; i < _chunks.Length; i++)
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int j = 0; j < _size; j++)
            {
                GameObject newObj = Instantiate(_chunks[i].gameObject);
                newObj.gameObject.SetActive(false);
                newPool.Enqueue(newObj);
            }
            _chunksQueueList.Add(newPool);
            
        }
    }
    private void SpawnRandomChunk()
    {
        if (Random.Range(0, 100) <= 10 + goldStageProbability)
        {
            GameObject newChunk = _chunksQueueList[ _chunksQueueList.Count-1].Dequeue();
            newChunk.transform.position = _spawnPos;
            _spawnPos.z += _chunkLenght;
            newChunk.gameObject.SetActive(true);
            _chunksQueueList[_chunksQueueList.Count-1].Enqueue(newChunk);
        }
        else
        {
            int randValue = Random.Range(0, _chunksQueueList.Count - 1);
        GameObject newChunk = _chunksQueueList[randValue].Dequeue();
        newChunk.transform.position = _spawnPos;
        _spawnPos.z += _chunkLenght;
        newChunk.gameObject.SetActive(true);
            _chunksQueueList[randValue].Enqueue(newChunk);
        }
    }
}
