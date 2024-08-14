using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed;
    public Vector3 distance;
    public Transform lookTarget;


    void LateUpdate()
    {
        Vector3 dPos = cameraTarget.position + distance;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos , sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }
}
