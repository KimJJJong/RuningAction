using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmazingAssets.CurvedWorld;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private float map_speed;

    [SerializeField]
    private Vector3 orientation;

    MapIndexManager mapIndexManager;
    MapObjectManager mapObjectManager;
    CurvedWorldController curvedController;

    [SerializeField]
    private Vector2 curvedSize;

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
            Debug.LogError("MapManager: Map Index Mangaer loading failed");

        mapObjectManager = FindObjectOfType<MapObjectManager>();
        if (!mapObjectManager)
            Debug.LogError("MapManager: Map Object Mangaer loading failed");
            //Todo: If two or more MOM, check
        
        curvedController = GameObject
            .Find("Curved World Controller")
            .GetComponent<CurvedWorldController>();
        if (!curvedController)
            Debug.LogError("CurvedController: Curved World Controller loading failed");
        else
        {
            curvedController.SetBendHorizontalSize(curvedSize.x);
            curvedController.SetBendVerticalSize(curvedSize.y);
        }

        mapIndexManager.activateNextMap();
        mapObjectManager.RegisterMapObjects(mapIndexManager.activated_list.Last());
    }

    // Update is called once per frame
    void Update()
    {
        if (is_sync_ready && gameManager.gameState == GameState.Playing)
        {
            MapPrefab firstMap = mapIndexManager.activated_list.First().GetComponent<MapPrefab>();
            if (firstMap.transform.position.x > transform.position.x + firstMap.prefab_size.x)
            {
                mapIndexManager.deactivateMap();
                mapObjectManager.DeregisterMapObjects(mapIndexManager.activated_list.First());
            }

            MapPrefab lastMap = mapIndexManager.activated_list.Last().GetComponent<MapPrefab>();
            if (lastMap.transform.position.x > transform.position.x)
            {
                mapIndexManager.activateNextMap();
                mapObjectManager.RegisterMapObjects(mapIndexManager.activated_list.Last());
            }

            //activatedlist�� �ִ� �� �����յ� �̵�
            foreach (GameObject obj in mapIndexManager.activated_list)
            {
                obj.transform.Translate(orientation.normalized * map_speed * Time.deltaTime);
            }

            //���̵� ���� ����
            map_speed += Time.deltaTime * 0.1f;
        }
    }

    IEnumerator SyncGameManager()
    {
        sync_time_passed = 0;

        while (!is_sync_ready)
        {
            gameManager = FindAnyObjectByType<GameManager>();

            if (gameManager)
            {
                is_sync_ready = true;
                map_speed = gameManager.GetMapSpeed();
            }
            else
            {
                sync_time_passed += sync_tick;

                if (sync_time_passed >= sync_threshold)
                {
                    Debug.LogError("MapManager: GameManager loading failed");
                    break;
                }
            }

            yield return new WaitForSeconds(sync_tick);
        }
    }
}
