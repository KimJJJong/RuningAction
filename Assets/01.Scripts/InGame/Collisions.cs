using System.Collections;
using TMPro;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    HpController hpController;
    new Camera camera;
    //cam movement
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;

    public bool isDmg;
    public bool isDmgIteam;
    public float chance;
    private Animator animator;
    public bool canInteract { get; set; }

    public TextMeshProUGUI cooldownText;

    PlayerController playerController;
    SoccerBall ball;

    void Start()
    {
        camera = FindAnyObjectByType<Camera>();
        hpController = FindAnyObjectByType<HpController>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        canInteract = true;
        cooldownText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleCollision(other);
        }
        else if (other.CompareTag("SoccerBall"))
        {
            ball = other.GetComponent<SoccerBall>();
        }
        else if (other.CompareTag("ShootingZone"))
        {
            Shoot();
        }
    }

    void ObstacleCollision(Collider obstacle)
    {
        isDmgIteam = Random.value <= chance;
        obstacle.gameObject.GetComponent<Collider>().enabled = false;

        bool isSliding = animator.GetCurrentAnimatorStateInfo(0).IsName("Slide");
        bool shouldTakeDamage = !isDmg && !isDmgIteam;

        if (isSliding)
        {
            if (obstacle.TryGetComponent(out IBonusObstacle bonusable))
            {
                bonusable.GetBonus();
            }
            else if (shouldTakeDamage)
            {
                ApplyDamage();
            }
        }
        else
        {
            if (shouldTakeDamage)
            {
                ApplyDamage();
            }
            else
            {
                GameManager.Instance.score.IncreasObsScore();
                Debug.Log("»Ñ¼ø°Ç°¡?");
            }
        }
    }

    void ApplyDamage()
    {
        Damage();
        cooldownText.enabled = true;
    }

    public void Damage()
    {
        if (canInteract)
        {
            animator.Play("Stumble");
            hpController.collsionObstacle();

            if (hpController.getValue() <= 0)
            {
                Die();
            }
            else
            {
                CamShake();
                GameManager.Instance.postEffectController.GetDamage();

            }
            StartCoroutine(Cooldown());
        }
    }

    void Shoot()
    {
        ball.Shoot(playerController.curPos);
    }

    IEnumerator Cooldown()
    {
        StartCoroutine(DieAnimation());
        canInteract = false;
        StartCoroutine(BlinkCooldownText());


        yield return new WaitForSeconds(2f);
        cooldownText.enabled = false;

        canInteract = true;
    }

    void Die()
    {
        animator.Play("Stumble");
        GameManager.Instance.GameOver();
    }

    public void CamShake()
    {
        StartCoroutine(Shake());
    }
    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(3);
    }

    IEnumerator BlinkCooldownText()
    {
        while (!canInteract)
        {
            cooldownText.enabled = !cooldownText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            camera.transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



}
