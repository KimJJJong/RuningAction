using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDribble : MonoBehaviour
{
    public float detectionRange = 10f;       //플레이어 감지 범위
    public float moveSpeed = 5f;             //이동 속도
    public float returnDelay = 3f;           //충돌 후 처음 위치로 돌아가는 시간
    public float disappearTime = 5f;         //장애물 사라졌다가 다시 나타나는 시간
    private Vector3 originalPosition;        //처음 위치 저장
    private bool isMoving = false;           //장애물 이동 여부
    private bool hasCollided = false;        //플레이어 충돌 여부
    private GameObject player;               //플레이어

    private Animator animator;

    void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isMoving && Vector3.Distance(player.transform.position, transform.position) <= detectionRange)
        {
            StartMoving();
        }

        if (isMoving && Vector3.Distance(player.transform.position, transform.position) > detectionRange)
        {
            StopMoving();
        }

        if (hasCollided)
        {
            StopMoving();
        }
    }

    void StartMoving()
    {
        isMoving = true;
        animator.SetBool("IsDribbling", true); // 공을 튕기며 이동하는 애니메이션 시작
        StartCoroutine(MoveTowardsPlayer());
    }

    IEnumerator MoveTowardsPlayer()
    {
        while (isMoving)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    void StopMoving()
    {
        isMoving = false;
        animator.SetBool("IsDribbling", false);
        StartCoroutine(ReturnToOriginalPosition());
    }

    IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(returnDelay);
        transform.position = originalPosition;
    }

    void Respawn()
    {
        gameObject.SetActive(true);
        transform.position = originalPosition;
    }
}
