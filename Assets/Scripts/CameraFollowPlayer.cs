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
        Vector3 sPos = Vector3.Lerp(new Vector3(0, transform.position.y, transform.position.z), new Vector3(0, cameraTarget.position.y + distance.y, cameraTarget.position.z + distance.z ), sSpeed * Time.deltaTime);
        transform.position = sPos;
    }
}
