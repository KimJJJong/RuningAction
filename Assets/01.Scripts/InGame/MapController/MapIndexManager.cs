using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIndexManager : MonoBehaviour
{
    //map 프리팹 저장소
    public MapPrefab[] map_poll;


    //private이어야 하는데 friend class 같은 기능 c#에서 찾기 귀찮아서 그냥 public해둠. 
    //todo: 나중에 private으로 교체
    //맵에 켜져 있는 맵 프리팹들
    public List<MapPrefab> activated_list;
    //서버에서 받아온 맵 인덱싱 순서
    private int[] map_order_list;
    //현재 맵 인덱싱 순서
    public int cur_order;    
    
    private void Awake()
    {
        //서버에서 mapIndexList 받아옴
        map_order_list[0] = 0;
        map_order_list[0] = 1;

        cur_order = 0;
    }
    
    //다음 맵 인덱스 가져옴
    public int getNextIndex()
    { 
        return map_order_list[cur_order]; 
    }

    //다음 맵 인덱스 활성화
    public void activateNextMap()
    {
        map_poll[getNextIndex()].GetComponent<Renderer>().enabled = true;
        activated_list.Add(map_poll[getNextIndex()]);
        ++cur_order;
    }

    //가장 앞에 있는 맵 비활성화 (지나갔을 때 부르기) 
    public void deactivateMap()
    {        
        resetMap(activated_list[0].getID());
        activated_list.RemoveAt(0);
    }

    //맵 비활성화 & 포지션 리셋
    public void resetMap(int index)
    {
        map_poll[index].GetComponent<Renderer>().enabled = false;
        //map_poll[index] 포지션 등 값들 초기화
    }



    #region JustInCase
    public void activateMap(int index)
    {
        activated_list.Add(map_poll[index]);       
    }

 
    public void deactivateMap(int index)
    {
        activated_list.Remove(map_poll[index]);    
    }

    #endregion
}
