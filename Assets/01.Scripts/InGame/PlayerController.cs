using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerPosition
    {
        public Transform front;
        public Transform back;
        public Transform retire;
    }

    private GameUIManager gameUiManager;

    public GameObject playerObj;
    public Animator animator;

    public PlayerPosition playerPosition;

    [Header("Move Stat")]
    public float sideSpeed;
    public float runningSpeed;
    public float maxSpeed = 22f;
    public float accelerationRate;
    public float jumpForce;
    public float downForce = 400;
    public float fallMultiplier = 3;

    [Space(10f)]
    [Header("Character")]
    public int idleType;

    bool smash;

    public int jumpCount = 0;
    public int maxJumpCount = 2;
    bool isSliding = false;

    bool _isRush;
    bool _isMagnetic;

    [HideInInspector]
    public int curPos = 1; // 0 = left, 1 = center, 2 = right;

    [HideInInspector]
    public CapsuleCollider col;

    private Rigidbody rb;

    [HideInInspector]
    public StrengthenSubstance substance;

    [HideInInspector]
    public CollectCoin score;

    public bool IsRush => _isRush;
    public bool IsMagnetic
    {
        get => _isMagnetic;
        set => _isMagnetic = value;
    }

    public bool autoDodge = false;
    public float dodgeDuration = 3f;

    private GameManager.Lane _lane;

    public GameManager.Lane lane
    {
        get => _lane;
        set => _lane = value;
    }

    MoveActionWorker actionWorker = new MoveActionWorker();

    private Ball ball;

    private bool _isDisabled = false;

    public bool isDisabled
    {
        get => _isDisabled;
    }

    [HideInInspector]
    public PlayerController leftController;

    [HideInInspector]
    public PlayerController rightController;

    void Awake()
    {
        col = playerObj.GetComponent<CapsuleCollider>();
        rb = playerObj.GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        substance = playerObj.GetComponent<StrengthenSubstance>();
        score = playerObj.GetComponent<CollectCoin>();
    }

    private void Start()
    {
        gameUiManager = GameUIManager.instance;
        ball = GameManager.Instance.playerManager.ball;
        SetIdleAnimation(idleType);

        HpController.OnHpZero(
            (playerObj) =>
            {
                if (this.playerObj != playerObj)
                    return;

                SetState(EState.Retire);

                if (leftController != null)
                    leftController.rightController = rightController;

                if (rightController != null)
                    rightController.leftController = leftController;

                if (GameManager.Instance.playerManager.GetCurrentController() == this)
                {
                    PlayerController to;
                    if (leftController != null)
                        to = leftController;
                    else if (rightController != null)
                        to = rightController;
                    else
                    {
                        GameManager.Instance.GameOver();
                        return;
                    }

                    GameManager.Instance.playerManager.Pass(this, to);
                }

                MoveRetire();
            }
        );
    }

    public PlayerController GetLeftController()
    {
        if (leftController == null)
            return null;

        return leftController;
    }

    public PlayerController GetRightController()
    {
        if (rightController == null)
            return null;

        return rightController;
    }

    public MoveActionWorker.MoveActionListener MoveFront()
    {
        return Move(playerPosition.front.position);
    }

    public MoveActionWorker.MoveActionListener MoveBack()
    {
        return Move(playerPosition.back.position);
    }

    public MoveActionWorker.MoveActionListener MoveRetire()
    {
        return Move(playerPosition.retire.position, 1.5f);
    }

    public MoveActionWorker.MoveActionListener Move(Vector3 targetPosition)
    {
        return Move(targetPosition, GameManager.Instance.playerManager.passSpeed);
    }

    public MoveActionWorker.MoveActionListener Move(Vector3 targetPosition, float duration)
    {
        Tweener tweener = DOTween.To(
            () => playerObj.transform.position,
            position =>
            {
                Vector3 tmp = playerObj.transform.position;
                tmp.x = position.x;
                tmp.y = position.y;
                tmp.z = position.z;
                playerObj.transform.position = tmp;
            },
            targetPosition,
            duration
        );

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(tweener)
            .Build();

        actionWorker.Clear().AddAction(action);

        return action.listener;
    }

    public MoveActionWorker.MoveActionListener Jump()
    {
        float jumpSpd = Math.Max(
            GameManager.Instance.playerManager.jumpDuration / GameManager.Instance.gameSpeed,
            0.5f
        );

        float jumpForce = GameManager.Instance.playerManager.jumpHeight;

        for (int i = 1; i < jumpCount; i++)
            jumpForce *= 0.6f;

        return Jump(jumpForce, jumpSpd);
    }

    public MoveActionWorker.MoveActionListener Jump(float force, float duration)
    {
        /* if (jumpCount >= maxJumpCount)
            return null; */

        int inJumpCnt = ++jumpCount;

        float jumpSpd = duration;

        Vector3 positionOffset = playerObj.transform.position;

        float jumpForce = force;

        Vector3 down = playerObj.transform.position - Vector3.up * playerObj.transform.position.y;
        Vector3 up = playerObj.transform.position + Vector3.up * jumpForce;

        //Jump-up
        Tweener jumpUp = DOTween
            .To(
                () => playerObj.transform.position,
                position =>
                {
                    Vector3 tmp = playerObj.transform.position;
                    tmp.x = position.x;
                    tmp.y = position.y;
                    tmp.z = position.z;
                    playerObj.transform.position = tmp;
                },
                up,
                jumpSpd * 0.5f
            )
            .SetEase(Ease.OutQuad);

        //Jump-down
        Tweener jumpDown = DOTween
            .To(
                () =>
                {
                    return playerObj.transform.position;
                },
                position =>
                {
                    Vector3 tmp = playerObj.transform.position;
                    tmp.x = position.x;
                    tmp.y = position.y;
                    tmp.z = position.z;
                    playerObj.transform.position = tmp;
                },
                down,
                jumpSpd * 0.5f
            )
            .SetEase(Ease.InQuad);

        bool ballFlag = GameManager.Instance.playerManager.GetCurrentController() == this;
        if (ballFlag)
        {
            Vector3 ballPos = new Vector3(ball.ballOffset.x, ball.ballOffset.y, ball.ballOffset.z);

            ballPos.y = up.y;
            //ball.Jump(ballPos, jumpSpd);
        }

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(jumpUp, jumpDown)
            .AddStartCallBack(() =>
            {
                jumpCount = inJumpCnt;

                if (jumpCount == 2)
                {
                    animator.SetBool("isJumping", true);
                    animator.SetTrigger("doubleJump");
                }
                else
                    animator.Play("Jumping", 0, 0f);
            })
            .AddCompleteCallback(() => { })
            .AddFinishCallBack(() =>
            {
                jumpCount = 0;

                animator.SetBool("isJumping", false);
            })
            .Build();

        actionWorker.Clear().AddAction(action);

        return action.listener;
    }

    public MoveActionWorker.MoveActionListener Slide()
    {
        if (isSliding)
            return null;

        float slideSpd = Math.Max(1f / GameManager.Instance.gameSpeed, 0.5f);

        Tweener tweener = DOTween
            .To(
                () => playerObj.transform.position,
                position =>
                {
                    Vector3 tmp = playerObj.transform.position;
                    tmp.x = position.x;
                    tmp.y = position.y;
                    tmp.z = position.z;
                    playerObj.transform.position = tmp;
                },
                playerObj.transform.position - Vector3.up * playerObj.transform.position.y,
                slideSpd
            )
            .SetEase(Ease.OutQuart);

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(tweener)
            .AddStartCallBack(() =>
            {
                col.height /= 2;
                col.center /= 2;

                isSliding = true;

                animator.Play("Slide");
                MasterAudio.PlaySound3DAtTransform("slide", transform);
            })
            .AddCompleteCallback(() => { })
            .AddFinishCallBack(() =>
            {
                col.height *= 2;
                col.center *= 2;

                isSliding = false;
            })
            .Build();

        isSliding = true;
        actionWorker.Clear().AddAction(action);

        return action.listener;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing) { }
    }

    public void SetRunningAnimation(bool isRun)
    {
        animator.SetBool("Run", isRun);

        if (!isRun)
        {
            animator.SetInteger("idle", idleType);
        }
    }

    public void SetIdleAnimation(int type)
    {
        animator.SetInteger("idle", type);
    }

    public void SetState(EState state)
    {
        if (_isDisabled)
            return;

        switch (state)
        {
            case EState.Runing:
                {
                    animator.Play("Run_Animation");
                }
                break;
            case EState.Left:
                {
                    animator.Play("MoveLeft");
                }
                break;
            case EState.Right:
                {
                    animator.Play("MoveRight");
                }
                break;
            case EState.Up:
                {
                    Jump();
                }
                break;
            case EState.Down:
                {
                    Slide();
                }
                break;
            case EState.Kick:
                {
                    animator.Play("Kick");
                }
                break;
            case EState.Batting:
                {
                    animator.Play("Hit");
                }
                break;
            case EState.Throw:
                {
                    animator.Play("Pitch");
                }
                break;
            case EState.Pass:
                {
                    if (jumpCount > 0)
                    {
                        animator.Play("Pass_Heading", 0, 0f);
                    }
                    else
                    {
                        animator.Play("Kick");
                    }
                }
                break;
            case EState.Retire:
                {
                    animator.SetTrigger("isRetire");
                    _isDisabled = true;
                }
                break;
        }
    }

    #region Special
    public void Invincibility(bool isIteam) { }

    public void BeMagnetic() //Iteam Magnetic
    { }

    #endregion
}

public enum EState
{
    Runing,
    Left,
    Right,
    Up,
    Down,
    Kick,
    Smash,
    Throw,
    Batting,
    Pass,
    Retire,
}

public enum EPlayerPosition
{
    Left,
    Center,
    Right,
}
