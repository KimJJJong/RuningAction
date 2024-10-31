using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleDribble : MonoBehaviour, IBonusObstacle
{
    [SerializeField] private float detectRange = 30f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject ball;
    private bool isMoving = false;

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

        StartDribblingBall();
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (!isMoving && playerDistance < detectRange)
        {
            isMoving = true;
            targetPosition = new Vector3(transform.position.x, transform.position.y, player.position.z);
        }

        if (isMoving)
        {
            MoveTowardsTarget(targetPosition);

            if (playerDistance > detectRange)
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
        yield return new WaitForSeconds(5f);
        transform.position = startPosition;
    }

    public void GetBonus()
    {
        Debug.LogError("get bonus");
        StopMoving();
        FadeOut();

        GameManager.Instance.score.IncreasObsScore();
    }

    private void FadeOut()
    {
        animator.Play("Stumble");
    }

    private void StartDribblingBall()
    {
        ball.transform.DOLocalMoveZ(1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        ball.transform.DOLocalRotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
