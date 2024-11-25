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
        //GameManager�� MapIndexManager�θ��� (2���� ���)
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
            //if ���� �Ÿ��� ª����; MapPrefab Ŭ������ prefab length�� ���� ���� ���� �Ÿ��� mapGenLengthThreshold���� ���� ���
            mapIndexManager.activateNextMap();

            //if ������ �� ������
            mapIndexManager.deactivateMap();

            //activatedlist�� �ִ� �� �����յ� �̵�
            foreach (MapPrefab mp in mapIndexManager.activated_list)
            {
                mp.transform.Translate(orientation * map_speed * Time.deltaTime);
            }

            //���̵� ���� ����
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

        //������ ���� ���� GameManager�� ������ �ʿ� ���⵵ ��
        map_speed = gameManager.GetMapSpeed();
        is_sync_ready = true;
    }


}
