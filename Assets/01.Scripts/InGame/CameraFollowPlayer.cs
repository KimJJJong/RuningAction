using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 distance = new Vector3(0, 0, 0);
    public bool isMove = false;
    public float smoothSpeed = 5f;

    public void StartCameraMove(Transform target)
    {
        cameraTarget = target;
        isMove = true;
    }

    void LateUpdate()
    {
        if (isMove && cameraTarget != null)
        {
            Vector3 targetPosition = new Vector3(cameraTarget.position.x + distance.x, cameraTarget.position.y + distance.y, cameraTarget.position.z + distance.z);

            float smoothX = Mathf.Lerp(transform.position.x, targetPosition.x, smoothSpeed * Time.deltaTime);
            transform.position = new Vector3(smoothX, targetPosition.y, targetPosition.z);
        }
    }
}
