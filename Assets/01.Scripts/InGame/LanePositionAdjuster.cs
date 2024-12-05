using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanePositionAdjuster : MonoBehaviour
{
    public GameManager.Lane lane;

    void Start()
    {
        SetPosition();
    }

    void SetPosition()
    {
        //TODO: 나중에 방향 돌릴거면 orientation 기반으로 변경
        if(lane != GameManager.Lane.NotDecidedYet)
            transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.lanePositions[lane]);        
    }
}
