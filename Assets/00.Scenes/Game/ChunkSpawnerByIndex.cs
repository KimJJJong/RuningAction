using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazingAssets.CurvedWorld;
using UnityEngine.Networking;
public class ChunkSpawnerByIndex : MonoBehaviour
{
    [SerializeField] private GameObject[] chunks;
    [SerializeField] private float chunkLength = 119f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnDistance = 299f;
    [SerializeField] private int[] chunkArrayTest;

    [Header("Pool Config")]
    [SerializeField] private int size;

    private Queue<GameObject>[] chunksQueue;
    private Vector3 spawnPos = new Vector3(0f, -38f, 0f);
    private int[] chunkArray;
    private int currentIndex = 0;

    #region curved world argument
    private CurvedWorldController curvedController;
    [Header("Curved World")]
    [SerializeField] private float curvedZMaxValue = 10;
    [SerializeField] private float curvedZMinValue = -10;
    private float currentCurvedValue;
    private float currentTime = 0f;
    private float curveTime = 10f;
    #endregion
    private void Start()
    {
        spawnPos = new Vector3(-0.4f, -39.25718f, 170);

        //서버에서 청크 받아오기
        //StartCoroutine(GetChunkFromServer());
        chunkArray = chunkArrayTest;
        PoolChunks(); 
        
        curvedController = GameObject.Find("Curved World Controller").GetComponent<CurvedWorldController>();
        currentCurvedValue = curvedController.bendHorizontalSize;
    }

    private void Update()
    {
        if (chunkArray == null || chunkArray.Length == 0)
            return;

        if (Vector3.Distance(playerTransform.position, spawnPos) < spawnDistance)
        {
            SpawnChunkBySequence();
        }

        currentTime += Time.deltaTime;
        if (currentTime > curveTime)
        {
            currentTime = 0;

            if (currentCurvedValue < 0)
                StartCoroutine(SetCurvedWorld(currentCurvedValue, curvedZMaxValue, 2));
            else
                StartCoroutine(SetCurvedWorld(currentCurvedValue, curvedZMinValue, 2));
        }
    }

    private void PoolChunks()
    {
        chunksQueue = new Queue<GameObject>[chunks.Length];

        for (int i = 0; i < chunks.Length; i++)
        {
            chunksQueue[i] = new Queue<GameObject>();

            for (int j = 0; j < size; j++)
            {
                GameObject newObj = Instantiate(chunks[i]);
                newObj.SetActive(false);
                chunksQueue[i].Enqueue(newObj);
            }
        }
    }

    private void SpawnChunkBySequence()
    {
        if (currentIndex >= chunkArray.Length)
            currentIndex = 0;

        int chunkIndex = chunkArray[currentIndex];
        GameObject chunk = chunksQueue[chunkIndex].Dequeue();
        chunk.transform.position = spawnPos;
        spawnPos.z += chunkLength;
        chunk.SetActive(true);
        chunksQueue[chunkIndex].Enqueue(chunk);

        currentIndex++;
    }

    //서버에서 청크 시퀀스를 받아오는 코루틴
    private IEnumerator GetChunkFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            chunkArray = JsonUtility.FromJson<int[]>(jsonResponse);
        }
    }

    private IEnumerator SetCurvedWorld(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            currentCurvedValue = Mathf.Lerp(startValue, endValue, t);
            curvedController.SetBendHorizontalSize(currentCurvedValue);

            yield return null;
        }

        currentCurvedValue = endValue;
    }
}
