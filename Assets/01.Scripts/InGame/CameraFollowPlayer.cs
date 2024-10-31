using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed;
    public Vector3 distance;
    public bool isMove = false;

    public void StartCameraMove()
    {
        cameraTarget = GameObject.Find("Player").GetComponent<Transform>();
        distance = new Vector3(0, 3, 1f);
        sSpeed = 4;


        isMove = true;
    }

    void LateUpdate()
    {
        if (isMove)
        {
            Vector3 sPos = Vector3.Lerp(transform.position, new Vector3(cameraTarget.position.x, cameraTarget.position.y + distance.y, cameraTarget.position.z + distance.z), sSpeed * Time.deltaTime);
            transform.position = sPos;
        }
    }
}
