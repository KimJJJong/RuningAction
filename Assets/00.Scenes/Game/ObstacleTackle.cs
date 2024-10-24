using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTackle : MonoBehaviour
{
    [SerializeField] private float detectRange = 30f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float slideRange = 5f;
    private bool isMoving = false;
    private bool isSliding = false;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Animator animator;
    private Transform player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (!isMoving && playerDistance < detectRange && !isSliding)
        {
            isMoving = true;
            targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }

        if (isMoving)
        {
            MoveTowardsTarget(targetPosition);

            if (playerDistance > detectRange)
            {
                isMoving = false;
                targetPosition = startPosition;

                StopMoving();
            }
            else if (playerDistance < slideRange && !isSliding)
            {
                StartCoroutine(SlideAndReturn());
            }
        }
    }

    void StopMoving()
    {
        isMoving = false;
        StartCoroutine(ReturnPosition());
    }

    private IEnumerator ReturnPosition()
    {
        yield return new WaitForSeconds(5f);
        transform.position = startPosition;
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.z), moveSpeed * Time.deltaTime);
    }

    private IEnumerator SlideAndReturn()
    {
        isSliding = true;
        animator.Play("Slide");
        yield return new WaitForSeconds(1f);

        targetPosition = startPosition;
        isSliding = false;

        StopMoving();
    }
}
