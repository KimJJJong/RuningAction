using System.Collections;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform ballPos;

    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;

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
    public ECharacter eCh;
    public ERank eChRank;

    [Space(10f)]
    [Header("Bat/Glove Ranks")]
    public ERank eBatRank;
    public ERank eGloveRank;

    bool smash;
    bool slide;
    bool isJumping = false;
    bool _isRush;
    bool _isMagnetic;
    [HideInInspector] public int curPos = 1;   // 0 = left, 1 = center, 2 = right;

    [HideInInspector] public CapsuleCollider col;
    [HideInInspector] public Collisions collisions;
    private Rigidbody rb;
    private Weapon weapon;

    public bool IsRush => _isRush;
    public bool IsMagnetic
    {
        get => _isMagnetic;
        set => _isMagnetic = value;
    }

    public bool autoDodge = false;
    public float dodgeDuration = 3f;

    void Awake()
    {
        collisions = GetComponent<Collisions>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            if (collisions.canInteract)
            {
                //Running();
                //StateUpdate();

                if (Input.GetKeyDown(KeyCode.Space))// && !autoDodge)
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
        }
    }

    public void SetRunningAnimation(bool isRun)
    {
        animator.SetBool("Run", isRun);
    }
    /*
    private void Running()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + runningSpeed * Time.deltaTime);
        transform.position = newPosition;

        runningSpeed += accelerationRate * Time.deltaTime;
        if (runningSpeed > maxSpeed)
        {
            runningSpeed = maxSpeed;
        }
    }

    private void MoveHorizontal()
    {
        if (!autoDodge)
        {
            if (curPos == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !IsObstacleLane(0))
            {
                SetState(EState.Left);
                curPos = 0;
            }
            else if (curPos == 1 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !IsObstacleLane(2))
            {
                SetState(EState.Right);
                curPos = 2;
            }
            else if (curPos == 0 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !IsObstacleLane(1))
            {
                SetState(EState.Right);
                curPos = 1;
            }
            else if (curPos == 2 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !IsObstacleLane(1))
            {
                SetState(EState.Left);
                curPos = 1;
            }
        }

        MoveToCenter();
    }

    void StateUpdate()
    {
        MoveHorizontal();


        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            SetState(EState.Up);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetState(EState.Down);

        }
        if (Input.GetKeyDown(KeyCode.K) && !smash)
        {
            SetState(EState.Throw);
        }
        if (Input.GetKeyDown(KeyCode.L) && !smash)
        {
            SetState(EState.Batting);
        }

    }

    private void MoveToCenter()
    {
        Vector3 targetPos = transform.position;

        if (curPos == 1) targetPos = new Vector3(centerPos.position.x, transform.position.y, transform.position.z);
        else if (curPos == 0) targetPos = new Vector3(leftPos.position.x, transform.position.y, transform.position.z);
        else if (curPos == 2) targetPos = new Vector3(rightPos.position.x, transform.position.y, transform.position.z);

        if (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            transform.Translate(dir * sideSpeed * Time.deltaTime, Space.World);
        }
    }

    */
    public void SetState(EState state)
    {
        switch (state)
        {
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
                    if (!isJumping)
                    {
                        isJumping = true;
                        rb.AddForce(Vector3.up * jumpForce);
                        animator.Play("Jumping");
                        MasterAudio.PlaySound3DAtTransform("jump 2", transform);
                    }
                }
                break;
            case EState.Down:
                {
                    if (!slide)
                    {
                        if (isJumping)
                        {
                            rb.velocity = Vector3.zero;
                            rb.AddForce(Vector3.down * downForce * 2f);
                        }

                        animator.Play("Slide");
                        slide = true;

                        MasterAudio.PlaySound3DAtTransform("side_move", transform);
                        StartCoroutine(MoveDownAndUp());
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
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            slide = false;
        }
    }

    #region Special
    public void Invincibility(bool isIteam)
    {
        if (!_isRush)
        {
            GameManager.Instance.postEffectController.RushPostEffect(0.125f, 0.25f, true);
            if (!isIteam)
                StartCoroutine(InvincibilityWeaponTimer());
            else if (isIteam)
                StartCoroutine(InvincibilityIteamTimer(5f + GameManager.Instance.substance.RushLv));
        }
    }

    public void BeMagnetic()    //Iteam Magnetic
    {
        StartCoroutine(MagneticTimer(5f + GameManager.Instance.substance.MagneticLv));
    }

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

    IEnumerator InvincibilityWeaponTimer()
    {
        _isRush = true;

        float tmpSpd = runningSpeed;
        runningSpeed = 30;
        GameManager.Instance.playController.isDmg = true;
        while (GameManager.Instance.player.GetComponent<Weapon>().GageSlider.value > 0)
        {
            GameManager.Instance.weapon.DecreaseGage(10f);
            yield return new WaitForSeconds(1);
        }
        GameManager.Instance.playController.isDmg = false;
        GameManager.Instance.postEffectController.RushPostEffect(0.15f, 0.2f, false);

        runningSpeed = tmpSpd;
        _isRush = false;

    }
    IEnumerator InvincibilityIteamTimer(float time)
    {
        _isRush = true;
        float tmpSpd = runningSpeed;
        runningSpeed = 30;
        GameManager.Instance.playController.isDmg = true;

        yield return new WaitForSeconds(time);
        GameManager.Instance.postEffectController.RushPostEffect(0.25f, 0.25f, false);

        GameManager.Instance.playController.isDmg = false;
        runningSpeed = tmpSpd;
        _isRush = false;
    }

    IEnumerator MagneticTimer(float time)
    {
        IsMagnetic = true;
        yield return new WaitForSeconds(time);
        IsMagnetic = false;
    }
    #endregion

    #region Dodge
    private IEnumerator AutoDodgeRoutine(float duration)
    {
        autoDodge = true;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.postEffectController.RushPostEffect(0.25f, 0.25f, false);
        //autoDodge = false;
    }

    public void DodgeOnPosition()
    {
        if (curPos == 0 || curPos == 2)
        {
            MoveToLane(1);
        }
        else if (curPos == 1)
        {
            if (!IsObstacleLane(0)) { MoveToLane(0); }
            else if (!IsObstacleLane(2)) { MoveToLane(2); }
        }
    }

    public void DodgeOnSlide()
    {
        if (!slide)
        {
            animator.Play("Slide");
            slide = true;
            if (isJumping)
            {
                rb.AddForce(Vector3.down * downForce);
            }
            MasterAudio.PlaySound3DAtTransform("side_move", transform);

            StartCoroutine(MoveDownAndUp());
        }
    }

    public void DodgeOnJump()
    {
        if (!isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            animator.Play("Jumping");
            MasterAudio.PlaySound3DAtTransform("jump 2", transform);
        }
    }

    private bool IsObstacleLane(int lane)
    {
        Vector3 lanePosition = lane == 0
            ? new Vector3(leftPos.position.x, transform.position.y, transform.position.z)
            : (lane == 1
                ? new Vector3(centerPos.position.x, transform.position.y, transform.position.z)
                : new Vector3(rightPos.position.x, transform.position.y, transform.position.z));

        Vector3 boxHalfExtents = new Vector3(0.5f, 1f, 0.5f);
        RaycastHit hit;

        bool hasObstacle = Physics.BoxCast(
            transform.position + new Vector3(0, 1f, 0),
            boxHalfExtents,
            (lanePosition - transform.position).normalized,
            out hit,
            Quaternion.identity,
            0.5f,
            LayerMask.GetMask("Obstacle")
        );

        if (hasObstacle && hit.collider.CompareTag("ObstacleWall"))
        {
            return true;
        }

        return false;
    }

    private void MoveToLane(int lane)
    {
        if (lane == 0)
        {
            SetState(EState.Left);
            curPos = 0;
        }
        else if (lane == 1)
        {
            SetState(EState.Right);
            curPos = 1;
        }
        else if (lane == 2)
        {
            SetState(EState.Right);
            curPos = 2;
        }
    }
    #endregion

    public void MoveToEmptyLane()
    {
        if (curPos != 0 && !IsObstacleLane(0))
        {
            SetState(EState.Left);
            curPos = 0;
        }
        else if (curPos != 1 && !IsObstacleLane(1))
        {
            SetState(EState.Right);
            curPos = 1;
        }
        else if (curPos != 2 && !IsObstacleLane(2))
        {
            SetState(EState.Right);
            curPos = 2;
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
}
