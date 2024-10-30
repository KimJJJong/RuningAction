using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Collisions : MonoBehaviour
{
    HpController hpController;

    public bool isDmg;
    public bool isDmgIteam;
    public float chance;
    private Animator animator;
    public bool canInteract { get; set; }

    public TextMeshProUGUI goalText;
    private PlayerController playerController;
    private SoccerBall ball;
    private float shootCooldown = 2f;
    private bool canShoot = true;

    private SkinnedMeshRenderer[] renderers;

    void Start()
    {
        hpController = FindAnyObjectByType<HpController>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        canInteract = true;
        goalText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Obstacle"))
        {
            if (playerController.autoDodge)
            {
                if (other.CompareTag("ObstacleSlide"))
                {
                    playerController.DodgeOnSlide();
                }
                else if (other.CompareTag("ObstacleJump"))
                {
                    playerController.DodgeOnJump();
                }
                else
                {
                    playerController.DodgeOnPosition();
                }
                return;
            }
            ObstacleCollision(other);

            if(other.CompareTag("ObstacleWall"))
            {
                playerController.MoveToEmptyLane();
            }
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
        if (playerController.autoDodge)
        {
            return;
        }

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
            //animator.Play("Stumble");
            hpController.collsionObstacle();

            if (hpController.getValue() <= 0)
            {
                Die();
            }
            else
            {
                GameManager.Instance.postEffectController.GetDamage();
            }

            BlinkPlayer();
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

    public void BlinkPlayer()
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                // Alpha Clip(Cutout) 적용을 위한 임계값 조정
                float originalCutoff = material.GetFloat("_Cutoff");
                float blinkCutoff = 0.8f;  // 높은 값일수록 투명해짐

                material.DOFloat(blinkCutoff, "_Cutoff", 0.1f)
                        .SetLoops(10, LoopType.Yoyo)
                        .OnComplete(() =>
                        {
                            material.SetFloat("_Cutoff", originalCutoff);
                            canInteract = true;
                            isDmg = false;
                        });
            }
        }
    }

    void Die()
    {
        animator.Play("Stumble");
        GameManager.Instance.GameOver();
    }
}
