using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const int NUM_PLAYER = 3;


    //0:LW, 1:ST, 2:RW    
    public Transform[] playerPositions;
    private Vector3[] cam_waypoints;

    public Vector3 initial_pos;
    public bool isMove = false;
    public float smoothSpeed = 5f;


    //Camera Values
    [SerializeField]
    private Vector3 distance = new Vector3(0, 0, 0);
    
    [SerializeField]
    private Quaternion cam_rotation;
    public Quaternion getCamRotation() { return cam_rotation; }

    [SerializeField]
    private int FOV;
    public int getFOV() {return FOV;}

    int lane_num;

    void Start()
    {
        cam_waypoints = new Vector3[NUM_PLAYER];        

        distance = new Vector3(3, 4.5f, 0);
        cam_rotation = Quaternion.Euler(30, -90, 0);
        FOV = 90;

        CalculateCameraPoints();

        initial_pos = cam_waypoints[1];

    }

    void CalculateCameraPoints()
    {
        for (int i = 0; i < NUM_PLAYER;  i++)
        {
            float cam_x = playerPositions[i].position.x + distance.x;
            float cam_y = playerPositions[i].position.y + distance.y;
            float cam_z = playerPositions[i].position.z + distance.z;

            cam_waypoints[i] = new Vector3(cam_x, cam_y, cam_z);                        
        }
    }

    public void CameraSetting()
    {
        transform.rotation = cam_rotation;
        GetComponent<Camera>().fieldOfView = 90;
        MoveCamera(1);

    }

    public void MoveCamera(int lane)
    {
        lane_num = lane;
        isMove = true;
    }

void LateUpdate()
    {
        if (isMove)
        {
            float smoothZ = Mathf.Lerp(
                transform.position.z,
                cam_waypoints[lane_num].z,
                smoothSpeed * Time.deltaTime
            );
            transform.position = new Vector3(cam_waypoints[lane_num].x, cam_waypoints[lane_num].y, smoothZ);
        }
    }

    // public IEnumerator MoveCamera(int lane_num)
    // {
    //     while(Vector3.Distance(transform.position, cam_waypoints[lane_num]) > 0.1f)
    //     {
    //     float smoothZ = Mathf.Lerp(
    //         transform.position.z,
    //         cam_waypoints[lane_num].z,
    //         smoothSpeed * Time.deltaTime);

    //     transform.position = new Vector3(
    //         cam_waypoints[lane_num].x, 
    //         cam_waypoints[lane_num].y, 
    //         smoothZ);             

    //         yield return null;
    //     }
    // }
}


