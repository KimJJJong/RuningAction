using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapIndexManager : MonoBehaviour
{
    public MapPrefab[] map_poll;
    private List<MapPrefab> map_list;
    private List<MapPrefab> clone_list;
    public List<MapPrefab> activated_list;
    private int[] map_order_list;
    public int cur_order;    
    
    private void Awake()
    {
        map_order_list = new int[3];
        map_order_list[0] = 0;
        map_order_list[1] = 0;
        map_order_list[2] = 0;


        //clone_list.Add();0만 넣음

        //map_poll[0].gameObject.SetActive(true);

        cur_order = 0;

        createMap();
    }

    private void createMap(){
        map_list = new List<MapPrefab>();
        
        Vector3 spawnPos = Vector3.zero;

        foreach(int idx in map_order_list){
            GameObject MapClone = Instantiate(map_poll[idx].gameObject);
            
            if(map_list != null && map_list.Count > 0){
                MapPrefab lastMap = map_list.Last();
                spawnPos = lastMap.gameObject.transform.position + new Vector3(0,0,lastMap.prefab_size.z);
                //spawnPos += map_list.Last().prefab_size;
            } 
            
            MapClone.transform.position = spawnPos;
            MapClone.SetActive(false);
            map_list.Add(MapClone.GetComponent<MapPrefab>());
        }
    }
    
    //���� �� �ε��� ������
    public int getNextIndex()
    { 
        return map_order_list[cur_order]; 
    }

    //���� �� �ε��� Ȱ��ȭ
    public void activateNextMap()
    {
        //map_poll[getNextIndex()].GetComponent<Renderer>().enabled = true;

        int idx = getNextIndex();
        if(idx >= map_list.Count){
            Debug.LogWarning("no next map");
            return;
        }
       
        MapPrefab map = map_list[idx];
        map.gameObject.SetActive(true);
        
        activated_list.Add(map);
        ++cur_order;
    }

    //���� �տ� �ִ� �� ��Ȱ��ȭ (�������� �� �θ���) 
    public void deactivateMap()
    {        
        resetMap(activated_list[0].getID());
        activated_list.RemoveAt(0);
    }

    //�� ��Ȱ��ȭ & ������ ����
    public void resetMap(int index)
    {
        map_list[index].gameObject.SetActive(false);
        //map_poll[index].GetComponent<Renderer>().enabled = false;
        //map_poll[index] ������ �� ���� �ʱ�ȭ
    }



    #region JustInCase
    public void activateMap(int index)
    {   
        activated_list.Add(map_list[index]);       
    }

 
    public void deactivateMap(int index)
    {
        activated_list.Remove(map_list[index]);    
    }

    #endregion
}
