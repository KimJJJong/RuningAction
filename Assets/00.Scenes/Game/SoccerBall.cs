using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float rotationSpeedY = 100f;
    [SerializeField] private float rotationSpeedZ = 100f;

    private bool isShooting = false;
    private ShotOnGoal shotOnGoal;

    private void Start()
    {
        shotOnGoal = FindAnyObjectByType<ShotOnGoal>();
    }

    void Update()
    {
        if (isShooting)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.forward * rotationSpeedZ * Time.deltaTime, Space.Self);
        }
    }

    public void Shoot(int playerLane)
    {
        isShooting = true;
        shotOnGoal.ShootEvent(playerLane);
    }

    public void Goal()
    {
        Debug.LogError("Goal");
        shotOnGoal.GoalEvent();

        isShooting = false;
    }

    public void Blocked()
    {
        Debug.LogError("Blocked");

        isShooting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalKeeper"))
        {
            Blocked();
        }
        else if (other.CompareTag("GoalPost"))
        {
            Goal();
        }

    }
}
