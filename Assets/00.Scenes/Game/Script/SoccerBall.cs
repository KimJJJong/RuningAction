using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public int ballLine;

    [SerializeField]
    private float moveSpeed = 50f;

    [SerializeField]
    private float rotationSpeed = 100f;

    [SerializeField]
    private float rotationSpeedY = 100f;

    [SerializeField]
    private float rotationSpeedZ = 100f;

    private bool isShooting = false;
    private ShotOnGoal shotOnGoal;

    private void Start()
    {
        shotOnGoal = transform.parent.GetComponent<ShotOnGoal>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (isShooting)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            GetComponent<MeshRenderer>().enabled = true;
            transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.forward * rotationSpeedZ * Time.deltaTime, Space.Self);
        }
    }

    public void Shoot(int playerLane)
    {
        isShooting = true;
        shotOnGoal.ShootEvent(playerLane);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalKeeper"))
        {
            isShooting = false;
            GetComponent<TrailRenderer>().enabled = false;
            shotOnGoal.PlayBlockEvent();
        }
        else if (other.CompareTag("GoalPost"))
        {
            isShooting = false;
            GetComponent<TrailRenderer>().enabled = false;
            shotOnGoal.PlayGoalEvent();
        }
    }
}
