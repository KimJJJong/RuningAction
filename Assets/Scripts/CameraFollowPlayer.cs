using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed;
    public Vector3 distance;

    private void Start()
    {
        cameraTarget = GameObject.Find("Player").GetComponent<Transform>();
        distance = new Vector3(0, 4, -3.5f);
        sSpeed = 4;
    }


    void LateUpdate()
    {
       // Vector3 dPos = cameraTarget.position + distance;
        Vector3 sPos = Vector3.Lerp(new Vector3(0, transform.position.y, transform.position.z), cameraTarget.position + distance, sSpeed * Time.deltaTime);
        transform.position = sPos;
       // transform.LookAt(lookTarget.position);
    }
}
