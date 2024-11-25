using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIndexManager : MonoBehaviour
{
    //map ������ �����
    public MapPrefab[] map_poll;


    //private�̾�� �ϴµ� friend class ���� ��� c#���� ã�� �����Ƽ� �׳� public�ص�. 
    //todo: ���߿� private���� ��ü
    //�ʿ� ���� �ִ� �� �����յ�
    public List<MapPrefab> activated_list;
    //�������� �޾ƿ� �� �ε��� ����
    private int[] map_order_list;
    //���� �� �ε��� ����
    public int cur_order;    
    
    private void Awake()
    {
        //�������� mapIndexList �޾ƿ�
        map_order_list[0] = 0;
        map_order_list[0] = 1;

        cur_order = 0;
    }
    
    //���� �� �ε��� ������
    public int getNextIndex()
    { 
        return map_order_list[cur_order]; 
    }

    //���� �� �ε��� Ȱ��ȭ
    public void activateNextMap()
    {
        map_poll[getNextIndex()].GetComponent<Renderer>().enabled = true;
        activated_list.Add(map_poll[getNextIndex()]);
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
        map_poll[index].GetComponent<Renderer>().enabled = false;
        //map_poll[index] ������ �� ���� �ʱ�ȭ
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
