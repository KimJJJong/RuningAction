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
    public IObjectPool<GameObject> map_poll { get; private set; }
    private List<MapPrefab> clone_list;
    public List<GameObject> activated_list = new List<GameObject>();
    private int[] map_order_list;
    public int cur_order;
    public int cur_map_idx;

    private void Awake()
    {
        map_poll = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            map_list.Count,
            map_list.Count
        );

        //오브젝트 미리 생성
        for (int i = 0; i < map_list.Count; i++)
        {
            cur_map_idx = i;
            GameObject obj = CreatePooledItem();
            MapPrefab mapPrefab = obj.GetComponent<MapPrefab>();
            mapPrefab.pool.Release(mapPrefab.gameObject);
        }

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

        //clone_list.Add();0만 넣음

        //map_poll[0].gameObject.SetActive(true);

        //createMap();
        cur_map_idx = 0;
        cur_order = 0;
    }

    #region ObjectPool func
    private GameObject CreatePooledItem()
    {
        GameObject mapObject = Instantiate(map_list[cur_map_idx].gameObject);
        MapPrefab mapPrefab = mapObject.GetComponent<MapPrefab>();
        mapPrefab.pool = map_poll;
        return mapObject;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        Vector3 spawnPos = Vector3.zero;
        if (activated_list.Count > 0)
        {
            MapPrefab lastMap = activated_list.Last().GetComponent<MapPrefab>();

            spawnPos = lastMap.transform.position - new Vector3(lastMap.prefab_size.x, 0, 0);
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


    private void createMap()
    {
        map_list = new List<MapPrefab>();

        Vector3 spawnPos = Vector3.zero;

        foreach (int idx in map_order_list)
        {
            //GameObject MapClone = Instantiate(map_poll[idx].gameObject);

            if (map_list != null && map_list.Count > 0)
            {
                MapPrefab lastMap = map_list.Last();
                spawnPos =
                    lastMap.gameObject.transform.position
                    + new Vector3(0, 0, lastMap.prefab_size.z);
                //spawnPos += map_list.Last().prefab_size;
            }

            //MapClone.transform.position = spawnPos;
            //MapClone.SetActive(false);
            //map_list.Add(MapClone.GetComponent<MapPrefab>());
        }
    }

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
        //map_poll[getNextIndex()].GetComponent<Renderer>().enabled = true;

        int idx = getNextIndex();
        if (0 > idx || idx >= map_list.Count)
        {
            //Debug.LogWarning("no next map");
            return;
        }
        cur_map_idx = idx;

        //MapPrefab map = map_list[cur_map_idx];
        map_poll.Get();
        //map.gameObject.SetActive(true);

        //activated_list.Add(map);
        ++cur_order;
    }

    public void deactivateMap()
    {
        //resetMap(activated_list[0].getID());
        //activated_list.RemoveAt(0);
        activated_list.First().GetComponent<MapPrefab>().ReleaseObject();
    }

    //�� ��Ȱ��ȭ & ������ ����
    public void resetMap(int index)
    {
        map_list[index].gameObject.SetActive(false);
        //map_poll[index].GetComponent<Renderer>().enabled = false;
        //map_poll[index] ������ �� ���� �ʱ�ȭ
    }
}
