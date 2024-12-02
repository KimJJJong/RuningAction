using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float rotating_speed;
    public Vector3 rotation_axis;
    
    void Start()
    {

    }
    
    void Update()
    {
        transform.Rotate(rotation_axis * rotating_speed * Time.deltaTime);
    }
}
