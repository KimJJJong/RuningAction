using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private float map_speed;

    [SerializeField]
    private Vector3 orientation;
    
    MapIndexManager mapIndexManager;
   
    float mapGenLengthThreshold = 100f;


    #region gameManagerSync

    GameManager gameManager = null;

    private bool is_sync_ready = false;
    private float sync_time_passed = 0;
    private float sync_threshold = 1.0f;
    private float sync_tick = .1f;

    #endregion

    void Start()
    {        
        //GameManager랑 MapIndexManager부르기 (2가지 방법)
        StartCoroutine(SyncGameManager());
        mapIndexManager = GetComponentInChildren<MapIndexManager>();

        if (!mapIndexManager)
            Debug.Log("MapManaer: Map Index Mangaer loading failed");


    }

    // Update is called once per frame
    void Update()
    {
        if(is_sync_ready)
        {
            //if 남은 거리가 짧으면; MapPrefab 클래스의 prefab length를 통해 현재 남은 거리가 mapGenLengthThreshold보다 작을 경우
            mapIndexManager.activateNextMap();

            //if 지나간 맵 있으면
            mapIndexManager.deactivateMap();

            //activatedlist에 있는 맵 프리팹들 이동
            foreach (MapPrefab mp in mapIndexManager.activated_list)
            {
                mp.transform.Translate(orientation * map_speed * Time.deltaTime);
            }

            //난이도 설정 예시
            map_speed += Time.deltaTime * 0.1f;
        }
    }

    IEnumerator SyncGameManager()
    {
        while (gameManager == null)
        {
            sync_time_passed += sync_tick;
            yield return new WaitForSeconds(sync_tick);

            if (sync_time_passed >= sync_threshold)
                Debug.Log("MapManager: GameManager loading failed");                   

        }
        
        gameManager = FindAnyObjectByType<GameManager>();

        //컨셉에 따라 굳이 GameManager랑 연결할 필요 없기도 함
        map_speed = gameManager.GetMapSpeed();
        is_sync_ready = true;
    }


}
