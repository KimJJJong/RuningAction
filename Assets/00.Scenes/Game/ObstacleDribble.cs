using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDribble : MonoBehaviour, IBonusObstacle
{
    [SerializeField] private float detectRange = 30f;
    [SerializeField] private float moveSpeed = 10f;
    private bool isMoving = false;
    private bool hasCollided = false;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Animator animator;
    private Transform player;

    public float fadeTime = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (!isMoving && playerDistance < detectRange && !hasCollided)
        {
            isMoving = true;
            targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }

        if (isMoving)
        {
            MoveTowardsTarget(targetPosition);

            if (playerDistance > detectRange || hasCollided)
            {
                StopMoving();
            }
        }
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.z), moveSpeed * Time.deltaTime);
    }

    void StopMoving()
    {
        targetPosition = startPosition;
        isMoving = false;
        StartCoroutine(ReturnPosition());
    }

    private IEnumerator ReturnPosition()
    {
        yield return new WaitForSeconds(2f);
        transform.position = startPosition;
    }

    public void GetBonus()
    {
        Debug.LogError("get bonus");
        StopMoving();
        FadeOut();

        hasCollided = true;
        GameManager.Instance.score.IncreasObsScore();
    }

    private void FadeOut()
    {
        Debug.LogError("FadeObstacle");
        animator.Play("Stumble");
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        transform.position = startPosition;
    }
}
