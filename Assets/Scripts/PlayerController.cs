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
    }
    [SerializeField] Transform centerPos;
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
    public float sideSpeed;
    public float runningSpeed;
    public float maxSpeed = 22f;
    public float accelerationRate;
    public float jumpForce;

    [Space(10f)]
    public int selectEWeapon; // eWeapons[ selectEWeapon ]
    EWeapon[] eWeapons = { EWeapon.Bat, EWeapon.Ball, EWeapon.Magnetic };

    bool smash;
    float targetPosition;
    float curPosition;
    bool isJumping = false;
    int curPos = 1;   // 0 = left, 1 = center, 2 = right;

    Collisions collisions;
    CapsuleCollider capsuleCollider;
    Rigidbody rb;
    Animator animator;
    Weapon weapon;

    void Start()
    {
        collisions = FindAnyObjectByType<Collisions>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
        GameObject.Find("Main Camera").AddComponent<CameraFollowPlayer>();


    }

    void Update()
    {
        if (collisions.canInteract)
        {
            Running();
            StateUpdate();
        }

    }



    void Running()
    {

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
        if (!isJumping)
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
                    animator.Play("Slide");

                }
                break;
            case EState.Smash:
                {
                    StartCoroutine(appearHitBox());
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

    public void Invincibility()
    {
        StartCoroutine(InvincibilityTimer());

    }


    IEnumerator appearHitBox()
    {
        smash = true;
        weapon.Triger(eWeapons[selectEWeapon]);
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

    IEnumerator InvincibilityTimer()
    {

        float tmpSpd = runningSpeed;
        runningSpeed = 30;
        GameManager.Instance.player.GetComponent<Collisions>().isDmg = true;
        while (GameManager.Instance.player.GetComponent<Weapon>().GageSlider.value > 0)
        {
            GameManager.Instance.player.GetComponent<Weapon>().GageSlider.value--;
            yield return new WaitForSeconds(1);
        }
        GameManager.Instance.player.GetComponent<Collisions>().isDmg = false;
        runningSpeed = tmpSpd;
    }

}
