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
    public int jumpCount = 0;
    public int maxJumpCount = 2;

    [Space(10f)]
    [Header("Character")]
    public ECharacter eCh;
    public ERank eChRank;

    [Space(10f)]
    [Header("Bat/Glove Ranks")]
    public ERank eBatRank;
    public ERank eGloveRank;

    bool smash;
    bool slide;

    bool _isRush;
    bool _isMagnetic;

    [HideInInspector]
    public int curPos = 1; // 0 = left, 1 = center, 2 = right;

    [HideInInspector]
    public CapsuleCollider col;

    [HideInInspector]
    public Collisions collisions;
    private Rigidbody rb;

    [HideInInspector]
    public StrengthenSubstance substance;

    [HideInInspector]
    public Weapon weapon;

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

    private EPlayerPosition _position;

    public EPlayerPosition position
    {
        get => _position;
        set => _position = value;
    }

    private Ball ball;

    private List<PlayerAction> action_list = new List<PlayerAction>();
    private UnityEvent OnActionClear = new UnityEvent();

    void Awake()
    {
        col = playerObj.GetComponent<CapsuleCollider>();
        rb = playerObj.GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        collisions = playerObj.GetComponent<Collisions>();
        collisions.setController(this);

        substance = playerObj.GetComponent<StrengthenSubstance>();
        weapon = playerObj.GetComponent<Weapon>();
        score = playerObj.GetComponent<CollectCoin>();
    }

    private void Start()
    {
        gameUiManager = GameUIManager.instance;
        ball = GameManager.Instance.playerManager.ball;
    }

    void ActionClear()
    {
        int loop = action_list.Count;
        while (loop > 0)
        {
            var action = action_list.First();
            if (action.Kill())
                action_list.Remove(action);

            loop--;
        }
        Debug.Log("ActionClear:" + action_list.Count);
    }

    public PlayerActionCallback MoveFront()
    {
        return Move(playerPosition.front.position);
    }

    public PlayerActionCallback MoveBack()
    {
        return Move(playerPosition.back.position);
    }

    public PlayerActionCallback Move(Vector3 targetPosition)
    {
        return Move(targetPosition, GameManager.Instance.playerManager.passSpeed);
    }

    public PlayerActionCallback Move(Vector3 targetPosition, float duration)
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

        PlayerActionCallback callback = new PlayerActionCallback(tweener);

        ActionClear();

        //callback.OnFinish
        //action_list.Add(new PlayerAction(tweener));
        action_list.Insert(0, new PlayerAction(tweener));

        return callback;
    }

    public PlayerActionCallback Jump()
    {
        if (jumpCount >= maxJumpCount)
            return null;

        int inJumpCnt = ++jumpCount;

        float jumpSpd = Math.Max(
            GameManager.Instance.playerManager.jumpSpeed / GameManager.Instance.gameSpeed,
            0.5f
        );

        Debug.Log("jumpSpd:" + jumpSpd / 2f);

        Vector3 positionOffset = playerObj.transform.position;

        Vector3 down = playerObj.transform.position - Vector3.up * playerObj.transform.position.y;
        Vector3 up =
            playerObj.transform.position
            + Vector3.up * GameManager.Instance.playerManager.jumpHeight;

        //
        /* Tweener tweener = DOTween.To(
            () => 0f,
            t =>
            {
                float jumpVal = 4 * t - 4 * t * t;
                Vector3 playerPos = playerObj.transform.position;
                playerPos.y =
                    positionOffset.y + jumpVal * GameManager.Instance.playerManager.jumpHeight;
                playerObj.transform.position = playerPos;
            },
            1f,
            jumpSpd
        ); */

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
                () => up,
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

        PlayerActionCallback callback = new PlayerActionCallback(jumpUp, jumpDown);

        callback
            .OnStart(() =>
            {
                jumpCount = inJumpCnt;

                //if (jumpCount == 1)
                animator.Play("Jumping");
            })
            .OnComplete(() =>
            {
                jumpCount = 0;
                animator.Play("Run_Animation");
            })
            .OnFinish(() =>
            {
                jumpCount = 0;
            });

        ActionClear();

        action_list.Add(new PlayerAction(jumpUp));
        action_list.Add(new PlayerAction(jumpDown));
        //action_list.Add(tweener);

        return callback;
    }

    public PlayerActionCallback Slide()
    {
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

        PlayerActionCallback callback = new PlayerActionCallback(tweener);
        callback
            .OnStart(() =>
            {
                col.height /= 2;
                col.center /= 2;
            })
            .OnComplete(() => { })
            .OnFinish(() =>
            {
                col.height *= 2;
                col.center *= 2;
            });

        ActionClear();

        action_list.Add(new PlayerAction(tweener));

        return callback;
    }

    public void ActionTask()
    {
        if (action_list.Count == 0)
            return;

        var action = action_list.First();

        //action.IsPlaying()

        if (action.GetTask().IsActive())
        {
            if (!action.GetTask().IsPlaying())
                action.GetTask().Play();
        }
        else
        {
            action_list.Remove(action);
        }
    }

    /* IEnumerator ActionTask_Cor(){
        return
    } */

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            if (collisions.canInteract)
            {
                //Running();
                //StateUpdate();

                if (Input.GetKeyDown(KeyCode.Space)) // && !autoDodge)
                {
                    //StartCoroutine(AutoDodgeRoutine(dodgeDuration));

                    autoDodge = !autoDodge;
                    if (autoDodge)
                        col.center = new Vector3(col.center.x, col.center.y, col.center.z + 0.5f);
                    else
                        col.center = new Vector3(col.center.x, col.center.y, col.center.z - 0.5f);

                    GameManager.Instance.postEffectController.RushPostEffect(0f, 0.25f, autoDodge);
                }
            }

            ActionTask();
        }
    }

    public void SetRunningAnimation(bool isRun)
    {
        animator.SetBool("Run", isRun);
    }

    public void SetState(EState state)
    {
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
                    if (jumpCount < maxJumpCount)
                    {
                        MasterAudio.PlaySound("jump_4");

                        jumpCount++;

                        Vector3 PlayerPosOffset = playerObj.transform.position;

                        bool ballFlag =
                            GameManager.Instance.playerManager.GetCurrentPlayer() == gameObject;

                        Debug.Log(
                            "jumpSpd:"
                                + Math.Max(
                                    GameManager.Instance.playerManager.jumpSpeed
                                        / GameManager.Instance.gameSpeed,
                                    0.5f
                                )
                        );
                        DOTween
                            .To(
                                () => 0f,
                                t =>
                                {
                                    float jumpVal = 4 * t - 4 * t * t;
                                    Vector3 playerPos = playerObj.transform.position;
                                    playerPos.y =
                                        PlayerPosOffset.y
                                        + jumpVal * GameManager.Instance.playerManager.jumpHeight;
                                    playerObj.transform.position = playerPos;

                                    if (ballFlag)
                                    {
                                        Vector3 ballPos = ball.transform.position;
                                        ballPos.y =
                                            ball.ballOffset.y
                                            + jumpVal
                                                * GameManager.Instance.playerManager.jumpHeight
                                                * 0.8f;
                                        ball.transform.position = ballPos;
                                    }
                                },
                                1f,
                                (
                                    Math.Max(
                                        GameManager.Instance.playerManager.jumpSpeed
                                            / GameManager.Instance.gameSpeed,
                                        0.5f
                                    )
                                )
                            )
                            .OnComplete(() =>
                            {
                                jumpCount = 0;
                                SetState(EState.Runing);
                            });

                        //rb.AddForce(Vector3.up * jumpForce);
                        animator.Play("Jumping");
                        //MasterAudio.PlaySound3DAtTransform("jump 2", transform);
                    }
                }
                break;
            case EState.Down:
                {
                    if (!slide)
                    {
                        /* if (jumpCount == 0)
                        {
                            rb.velocity = Vector3.zero;
                            rb.AddForce(Vector3.down * downForce * 2f);
                        } */

                        Slide()
                            .OnFinish(() =>
                            {
                                slide = false;
                            });
                        animator.Play("Slide");
                        slide = true;

                        MasterAudio.PlaySound3DAtTransform("slide", transform);

                        //StartCoroutine(MoveDownAndUp());
                    }
                }
                break;
            case EState.Kick:
                {
                    animator.Play("Kick");
                }
                break;
            case EState.Batting:
                {
                    StartCoroutine(appearHitBox(eChRank, ECharacter.Glove));
                    animator.Play("Hit");
                }
                break;
            case EState.Throw:
                {
                    StartCoroutine(appearHitBox(eChRank, ECharacter.Bat));
                    animator.Play("Pitch");
                }
                break;
            case EState.Pass:
                {
                    animator.Play("Kick");
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Score"))
        {
            gameUiManager.UpdateScoreText(500);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isJumping = false;
            slide = false;
        }
    }

    #region Special
    public void Invincibility(bool isIteam) { }

    public void BeMagnetic() //Iteam Magnetic
    { }

    IEnumerator appearHitBox(ERank chRank, ECharacter ch)
    {
        smash = true;
        weapon.Triger(eCh, chRank, ch);
        yield return new WaitForSeconds(1f);
        smash = false;
    }

    IEnumerator MoveDownAndUp()
    {
        col.height /= 2;
        col.center /= 2;

        yield return new WaitForSeconds(1f);

        slide = false;
        col.height *= 2;
        col.center *= 2;
    }

    #endregion


    class PlayerAction
    {
        Tweener tweener;

        //bool killable = true;
        KillableSetter isKillable = () => true;

        public PlayerAction(Tweener tweener)
        {
            this.tweener = tweener;
            //this.killable = true;
        }

        public PlayerAction(Tweener tweener, bool killable)
        {
            this.tweener = tweener;
            this.isKillable = () => killable;
        }

        public PlayerAction(Tweener tweener, KillableSetter killableSetter)
        {
            this.tweener = tweener;
            this.isKillable = killableSetter;
        }

        public bool Kill()
        {
            bool result = isKillable();
            if (result)
                tweener.Kill();

            return result;
        }

        public Tweener GetTask()
        {
            return tweener;
        }

        public delegate bool KillableSetter();
    }

    public class PlayerActionCallback
    {
        public delegate void ActionCallback();

        public event ActionCallback onStart;
        public event ActionCallback onComplete;
        public event ActionCallback onFinish;

        public PlayerActionCallback() { }

        public PlayerActionCallback(params Tweener[] tweeners)
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                bool isFirst = i == 0;
                bool isLast = i == tweeners.Length - 1;

                Tweener tweener = tweeners[i].Pause().SetAutoKill(true);

                if (isFirst)
                {
                    tweener.OnStart(() => onStart?.Invoke());
                }

                if (isLast)
                {
                    tweener.OnComplete(() => onComplete?.Invoke()).OnKill(() => onFinish?.Invoke());
                }
            }
        }

        public PlayerActionCallback OnStart(ActionCallback callback)
        {
            onStart += callback;
            return this;
        }

        public PlayerActionCallback OnComplete(ActionCallback callback)
        {
            onComplete += callback;
            return this;
        }

        public PlayerActionCallback OnFinish(ActionCallback callback)
        {
            onFinish += callback;
            return this;
        }

        public void OnStartCall()
        {
            onStart?.Invoke();
        }
    }
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
}

public enum EPlayerPosition
{
    Left,
    Center,
    Right,
}
