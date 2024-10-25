using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

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
    public TextMeshProUGUI goalText;
    private PlayerController playerController;
    private SoccerBall ball;
    private float shootCooldown = 2f;
    private bool canShoot = true;

    private SkinnedMeshRenderer[] renderers;

    void Start()
    {
        camera = FindAnyObjectByType<Camera>();
        hpController = FindAnyObjectByType<HpController>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        canInteract = true;
        cooldownText.enabled = false;
        goalText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleCollision(other);
        }
        else if (other.CompareTag("SoccerBall"))
        {
            if (ball == null && canShoot)
            {
                ball = other.GetComponent<SoccerBall>();
                Shoot();
            }
        }
    }

    void ObstacleCollision(Collider obstacle)
    {
        isDmgIteam = Random.value <= chance;

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
    }

    public void Damage()
    {
        if (canInteract)
        {
            isDmg = true;
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
        if (ball != null)
        {
            ball.Shoot(ball.ballLine);
            ball = null;
            StartCoroutine(ShootCooldown());
        }
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator Cooldown()
    {
        canInteract = false;
        cooldownText.enabled = true;

        //StartCoroutine(BlinkCooldownText());
        cooldownText.GetComponent<DOTweenAnimation>().DOPlay();

        yield return new WaitForSeconds(2f);
        cooldownText.enabled = false;
        canInteract = true;

        BlinkPlayer();
        yield return new WaitForSeconds(2f);
        isDmg = false;
    }

    public void BlinkPlayer()
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                material.DOFade(0, 0.2f).SetLoops(10, LoopType.Yoyo).OnComplete(() =>
                {
                    material.DOFade(1, 0.1f);
                });
            }
        }
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
