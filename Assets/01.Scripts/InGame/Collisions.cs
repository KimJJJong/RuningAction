using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class Collisions : MonoBehaviour
{
    HpController hpController;

    public bool canInteract;
    //public bool isDmg;
    //public bool isDmgItem;
    public float chance;
    private Animator animator;

    private PlayController playController;
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
        playController = FindAnyObjectByType<PlayController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (canInteract)
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

                if (other.CompareTag("ObstacleWall"))
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
    }

    void ObstacleCollision(Collider obstacle)
    {
        if (playerController.autoDodge)
        {
            return;
        }

        playController.isDmgItem = Random.value <= chance;

        bool isSliding = playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("Slide");
        bool shouldTakeDamage = !playController.isDmg && !playController.isDmgItem;

        if (isSliding)
        {
            if (obstacle.TryGetComponent(out IBonusObstacle bonusable))
            {
                bonusable.GetBonus();
            }
            else if (shouldTakeDamage)
            {
                Damage();
            }
        }
        else
        {
            if (shouldTakeDamage)
            {
                Damage();
            }
        }
    }

    public void Damage()
    {
        playController.isDmg = true;

        hpController.CollsionObstacle();

        if (hpController.GetValue() <= 0)
        {
            if (GameManager.Instance.gameState != GameState.GameOver)
            {
                Die();
            }
        }
        else
        {
            GameManager.Instance.postEffectController.GetDamage();
        }

        BlinkPlayer();
    }

    void Shoot()
    {
        if (ball != null)
        {
            playerController.SetState(EState.Kick);
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
                        playController.isDmg = false;
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
