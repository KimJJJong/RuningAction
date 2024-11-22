using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_move : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float speed;


    private void Start()
    {
        direction.Normalize();        

    }

    // Update is called once per frame
    void Update()
    {        
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
