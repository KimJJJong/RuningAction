using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDribble : MonoBehaviour
{
    public float detectionRange = 10f;       //�÷��̾� ���� ����
    public float moveSpeed = 5f;             //�̵� �ӵ�
    public float returnDelay = 3f;           //�浹 �� ó�� ��ġ�� ���ư��� �ð�
    public float disappearTime = 5f;         //��ֹ� ������ٰ� �ٽ� ��Ÿ���� �ð�
    private Vector3 originalPosition;        //ó�� ��ġ ����
    private bool isMoving = false;           //��ֹ� �̵� ����
    private bool hasCollided = false;        //�÷��̾� �浹 ����
    private GameObject player;               //�÷��̾�

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
        animator.SetBool("IsDribbling", true); // ���� ƨ��� �̵��ϴ� �ִϸ��̼� ����
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
