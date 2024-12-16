using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class MapIndexManager : MonoBehaviour
{
    public List<MapPrefab> map_list;
    public List<IObjectPool<GameObject>> map_poll = new List<IObjectPool<GameObject>>();
    private List<MapPrefab> clone_list;
    public List<GameObject> activated_list = new List<GameObject>();

    private int[] map_order_list;
    public int cur_order;
    public int cur_map_idx;

    private void Awake()
    {
        foreach (MapPrefab mapPrefab in map_list)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: CreatePooledItem,
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                defaultCapacity: 1,
                maxSize: 1
            );
            map_poll.Add(pool);
        }

        //오브젝트 미리 생성
        for (int i = 0; i < map_list.Count; i++)
        {
            cur_map_idx = i;
            GameObject obj = CreatePooledItem();
            MapPrefab mapPrefab = obj.GetComponent<MapPrefab>();
            mapPrefab.pool.Release(mapPrefab.gameObject);
        }

        setMapPattern();

        cur_map_idx = 0;
        cur_order = 0;
    }

    private void setMapPattern()
    {
        map_order_list = new int[10];
        map_order_list[0] = 0;
        map_order_list[1] = 0;
        map_order_list[2] = 0;
        map_order_list[3] = 0;
        map_order_list[4] = 0;
        map_order_list[5] = 0;
        map_order_list[6] = 0;
        map_order_list[7] = 0;
        map_order_list[8] = 0;
        map_order_list[9] = 0;
    }

    #region ObjectPool func
    private GameObject CreatePooledItem()
    {
        GameObject mapObject = Instantiate(map_list[cur_map_idx].gameObject);
        MapPrefab mapPrefab = mapObject.GetComponent<MapPrefab>();
        mapPrefab.pool = map_poll[cur_map_idx];
        return mapObject;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        Vector3 spawnPos = Vector3.zero;
        if (activated_list.Count > 0)
        {
            MapPrefab lastMap = activated_list.Last().GetComponent<MapPrefab>();

            MapPrefab nextMap = obj.GetComponent<MapPrefab>();
            float offset =
                nextMap.prefab_bounds.size.x
                - lastMap.prefab_bounds.min.x
                + nextMap.prefab_bounds.min.x;
            spawnPos = lastMap.transform.position - new Vector3(offset, 0, 0);
        }

        activated_list.Add(obj);

        obj.transform.position = spawnPos;
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        activated_list.Remove(obj);
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        activated_list.Remove(obj);
        Destroy(obj);
    }
    #endregion


    public int getNextIndex()
    {
        if (map_order_list.Length <= cur_order)
        {
            Debug.LogWarning("no next map");
            return -1;
        }

        return map_order_list[cur_order];
    }

    public void activateNextMap()
    {
        int idx = getNextIndex();
        if (0 > idx || idx >= map_list.Count)
        {
            //Debug.LogWarning("no next map");
            return;
        }
        cur_map_idx = idx;

        map_poll[cur_map_idx].Get();
        ++cur_order;
    }

    public void deactivateMap()
    {
        activated_list.First().GetComponent<MapPrefab>().ReleaseObject();
    }
}
