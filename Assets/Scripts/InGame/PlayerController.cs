using System.Collections;
using UnityEngine;

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
    public float downForce;
    public float fallMultiplier;

    [Space(10f)]
    //public int selectEWeapon; // eWeapons[ selectEWeapon ]
    public EWeapon eWeapons;//= { EWeapon.Bat, EWeapon.Ball, EWeapon.Magnetic };
    public int weaponLv =0;

    bool smash;
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
        if (Input.GetKeyDown(KeyCode.Space) && !smash)
        {
            SetState(EState.Smash);
        }
        if(Input.GetKeyDown(KeyCode.K) && !smash)
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
                    animator.Play("Jump");

                }
                break;
            case EState.Down:
                {
                    StartCoroutine(MoveDownAndUp());
                    if( isJumping)
                    {
                        rb.AddForce(Vector3.down * downForce);
                    }
                    animator.Play("Slide");

                }
                break;
            case EState.Throw:
                {
                    StartCoroutine(appearHitBox(0));
                }
                break;
            case EState.Batting:
                {
                    StartCoroutine(appearHitBox(1));
                }
                break;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    public void Invincibility(bool isIteam)
    {
        if (!_isRush)
        {

        if (!isIteam)
            StartCoroutine(InvincibilityWeaponTimer());
        else if(isIteam)
            StartCoroutine(InvincibilityIteamTimer(5f + GameManager.Instance.substance.RushLv));

        }
    }
    public void BeMagnetic()    //Iteam Magnetic
    {
        StartCoroutine(MagneticTimer(5f + GameManager.Instance.substance.MagneticLv));
    }


    IEnumerator appearHitBox(int weaponNumber)  // 0 = Throw  1 = Batting
    {
        smash = true;
        weapon.Triger(eWeapons, weaponNumber);
        yield return new WaitForSeconds(1f);
        smash = false;

    }
    IEnumerator MoveDownAndUp()
    {
        capsuleCollider.height /= 2;
        capsuleCollider.center /= 2;

        yield return new WaitForSeconds(1f);

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
        _isRush =false;
    }

    IEnumerator MagneticTimer(float time)
    {
        IsMagnetic = true;
        yield return new WaitForSeconds(time);
        IsMagnetic=false;

    }
}
