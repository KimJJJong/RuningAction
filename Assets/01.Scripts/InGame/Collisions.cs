using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

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
            hpController.CollsionObstacle();

            if (hpController.GetValue() <= 0)
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
            playerController.SetState(PlayerController.EState.Kick);
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
                Color originalColor = material.color;
                Color targetColor = new Color(145, 0, 0, 255f);

                material.DOColor(targetColor, 0.2f)
                    .SetLoops(10, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        material.color = originalColor;
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
