using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTackle : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private float detectRange = 30f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private float slideRange = 5f;
    private bool isMoving = false;
    private bool isReturning = false;
    private bool isSliding = false;
    private Vector3 targetPosition;
    private Animator animator;
    private Transform player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        targetPosition = startPosition.position;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (!isMoving && playerDistance < detectRange)
        {
            isMoving = true;
            isReturning = false;
            targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }

        if (isMoving)
        {
            MoveTowardsTarget(targetPosition, moveSpeed);

            if (playerDistance > detectRange)
            {
                isMoving = false;
                isReturning = true;
                targetPosition = startPosition.position;
            }
            else if (playerDistance < slideRange && !isSliding)
            {
                StartCoroutine(SlideAndReturn());
            }
        }

        if (isReturning)
        {
            MoveTowardsTarget(targetPosition, returnSpeed);
            if (Vector3.Distance(transform.position, startPosition.position) < 0.1f)
            {
                isReturning = false;
            }
        }
    }

    private void MoveTowardsTarget(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.z), speed * Time.deltaTime);
    }

    private IEnumerator SlideAndReturn()
    {
        isSliding = true;
        animator.Play("Slide");
        yield return new WaitForSeconds(1f);

        isReturning = true;
        targetPosition = startPosition.position;
        isSliding = false;
    }
}
