using System.Collections;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerController : MonoBehaviour
{
    enum EState
    {
        Runing,
        Left,
        Right,
        Up,
        Down,
        Smash,
        Throw,
        Batting,
    }
    [SerializeField] Transform centerPos;
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
    public float sideSpeed;
    public float runningSpeed;
    public float maxSpeed = 22f;
    public float accelerationRate;
    public float jumpForce;
    public float downForce = 400;
    public float fallMultiplier = 3;

    [Space(10f)]
    [Header("#캐릭터 ")]
    public ECharacter eCh;      // = { 캐릭터 }; 연결 필요
    public ERank eChRank;       // 캐릭터 Lv
                                //public int chStar;        //별의 갯수??

    [Space(10f)]
    [Header("#메인 무기 ( Bat / Glove ) ")]
    [Space(5f)]
    public ERank eBatRank;   // Glove레어도
    //public int batStar     //Bat별의 갯수

    [Space(5f)]
    public ERank eGloveRank;   // Glove레어도
    //public int gloveStar     //Glove별의 갯수


    bool smash;
    bool slide;
    float targetPosition;
    float curPosition;
    bool isJumping = false;
    bool _isRush;
    bool _isMagnetic;
    int curPos = 1;   // 0 = left, 1 = center, 2 = right;

    Collisions collisions;
    CapsuleCollider capsuleCollider;
    Rigidbody rb;
    Animator animator;
    Weapon weapon;

    public bool IsRush => _isRush;
    public bool IsMagnetic
    {
        get => _isMagnetic;
        set => _isMagnetic = value;
    }


    void Start()
    {
        collisions = FindAnyObjectByType<Collisions>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
        GameObject.Find("Main Camera").AddComponent<CameraFollowPlayer>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

    }

    void Update()
    {
        if (collisions.canInteract)
        {
            Running();
            StateUpdate();
        }

    }
    /*
         private void FixedUpdate()
        {
            RaycastHit hit;

            Vector3 rayOrigin = transform.position + new Vector3(0,0.1f,0);
            Vector3 rayDirection = Vector3.forward;


            if (Physics.Raycast(rayOrigin, rayDirection, out hit, 0.2f))
            {
                if (hit.collider.CompareTag("Ramp"))
                {
                    Vector3 newPos = hit.point;
                    transform.position = newPos;

                    Debug.Log("hit");
                }
            }
        }
     */


    void Running()
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

        float xPosition = transform.position.x;

    }
    private void MoveHorizontal()
    {
        if (curPos == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            SetState(EState.Left);
            curPos = 0;

        }
        else if (curPos == 1 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            SetState(EState.Right);
            curPos = 2;

        }
        else if (curPos == 0 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            SetState(EState.Right);
            curPos = 1;

        }
        else if (curPos == 2 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            SetState(EState.Left);
            curPos = 1;

        }
        MoveToCenter();
    }




    void StateUpdate()
    {
        // if (!isJumping)       // 점프중 좌, 우 이동 통제
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
        if (curPos == 1)
        {
            if (Vector3.Distance(transform.position, new Vector3(centerPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
            {
                Vector3 dir = new Vector3(centerPos.position.x, transform.position.y, transform.position.z) - transform.position;
                transform.Translate(dir.normalized * sideSpeed * Time.deltaTime, Space.World);
            }
        }
        else if (curPos == 0)
        {
            if (Vector3.Distance(transform.position, new Vector3(leftPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
            {
                Vector3 dir = new Vector3(leftPos.position.x, transform.position.y, transform.position.z) - transform.position;
                transform.Translate(dir.normalized * sideSpeed * Time.deltaTime, Space.World);
            }
        }
        else if (curPos == 2)
        {
            if (Vector3.Distance(transform.position, new Vector3(rightPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
            {
                Vector3 dir = new Vector3(rightPos.position.x, transform.position.y, transform.position.z) - transform.position;
                transform.Translate(dir.normalized * sideSpeed * Time.deltaTime, Space.World);
            }
        }
    }


    void SetState(EState state)
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
                    isJumping = true;
                    rb.AddForce(Vector3.up * jumpForce);
                    animator.Play("Jumping");
                    MasterAudio.PlaySound3DAtTransform("jump 2",transform); 
                }
                break;
            case EState.Down:
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

    public void Invincibility(bool isIteam)
    {
        if (!_isRush)
        {

            if (!isIteam)   // 특수효과 사용할때 Rush
                StartCoroutine(InvincibilityWeaponTimer());
            else if (isIteam)    // 아이템 먹어서 Rush
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
        yield return new WaitForSeconds(1f);    // 무기 coolTime
        smash = false;
    }
    IEnumerator MoveDownAndUp()
    {
        capsuleCollider.height /= 2;
        capsuleCollider.center /= 2;

        yield return new WaitForSeconds(1f);

        slide = false;
        capsuleCollider.height *= 2;
        capsuleCollider.center *= 2;
    }

    IEnumerator InvincibilityWeaponTimer()
    {
        _isRush = true;

        float tmpSpd = runningSpeed;
        runningSpeed = 30;
        GameManager.Instance.player.GetComponent<Collisions>().isDmg = true;
        while (GameManager.Instance.player.GetComponent<Weapon>().GageSlider.value > 0)
        {
            GameManager.Instance.weapon.DecreaseGage(10f);   // Weapon사용 질주 시간 통제
            yield return new WaitForSeconds(1);
        }
        GameManager.Instance.player.GetComponent<Collisions>().isDmg = false;
        runningSpeed = tmpSpd;
        _isRush = false;

    }
    IEnumerator InvincibilityIteamTimer(float time)
    {
        _isRush = true;
        float tmpSpd = runningSpeed;
        runningSpeed = 30;
        GameManager.Instance.player.GetComponent<Collisions>().isDmg = true;

        yield return new WaitForSeconds(time);

        GameManager.Instance.player.GetComponent<Collisions>().isDmg = false;
        runningSpeed = tmpSpd;
        _isRush = false;
    }

    IEnumerator MagneticTimer(float time)
    {
        IsMagnetic = true;
        yield return new WaitForSeconds(time);
        IsMagnetic = false;

    }
}
