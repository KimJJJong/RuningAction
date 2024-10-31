using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    [SerializeField] Transform centerPos;
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
    [SerializeField] GameObject centerPlayer;
    [SerializeField] GameObject leftPlayer;
    [SerializeField] GameObject rightPlayer;

    public float sideSpeed;
    public float runningSpeed;
    public float maxSpeed = 22f;
    public float accelerationRate;
    public float jumpForce;
    public float downForce = 400;
    public float fallMultiplier = 3;

    private Animator centerAnimator, leftAnimator, rightAnimator;
    private int currentPlayer = 1; // 0 = ¿ÞÂÊ, 1 = Áß¾Ó, 2 = ¿À¸¥ÂÊ
    private bool isJumping = false;
    private bool slide = false;

    void Start()
    {
        centerAnimator = centerPlayer.GetComponent<Animator>();
        leftAnimator = leftPlayer.GetComponent<Animator>();
        rightAnimator = rightPlayer.GetComponent<Animator>();

        SetInitialPositions();
    }

    void Update()
    {
        Running();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchPlayer(-1);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchPlayer(1);
        }
    }

    private void SetInitialPositions()
    {
        leftPlayer.transform.position = new Vector3(leftPos.position.x, transform.position.y, transform.position.z);
        centerPlayer.transform.position = new Vector3(centerPos.position.x, transform.position.y, transform.position.z + 1);
        rightPlayer.transform.position = new Vector3(rightPos.position.x, transform.position.y, transform.position.z);
    }

    private void Running()
    {
        Vector3 newPosition = transform.position + Vector3.forward * runningSpeed * Time.deltaTime;
        transform.position = newPosition;

        runningSpeed += accelerationRate * Time.deltaTime;
        runningSpeed = Mathf.Clamp(runningSpeed, 0, maxSpeed);
    }

    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            GetCurrentAnimator().Play("Jumping");
            Rigidbody rb = GetCurrentPlayer().GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Slide()
    {
        if (!slide)
        {
            slide = true;
            GetCurrentAnimator().Play("Slide");
            StartCoroutine(ResetSlide());
        }
    }

    private void SwitchPlayer(int direction)
    {
        int newPlayer = currentPlayer + direction;
        if (newPlayer < 0 || newPlayer > 2) return;

        Transform newPos = (newPlayer == 0) ? leftPos : (newPlayer == 1) ? centerPos : rightPos;
        Transform currentPos = (currentPlayer == 0) ? leftPos : (currentPlayer == 1) ? centerPos : rightPos;

        GetCurrentPlayer().transform.position = new Vector3(currentPos.position.x, transform.position.y, transform.position.z);
        currentPlayer = newPlayer;
        GetCurrentPlayer().transform.position = new Vector3(newPos.position.x, transform.position.y, transform.position.z + 1); 
    }

    private GameObject GetCurrentPlayer()
    {
        return (currentPlayer == 0) ? leftPlayer : (currentPlayer == 1) ? centerPlayer : rightPlayer;
    }

    private Animator GetCurrentAnimator()
    {
        return (currentPlayer == 0) ? leftAnimator : (currentPlayer == 1) ? centerAnimator : rightAnimator;
    }

    private IEnumerator ResetSlide()
    {
        yield return new WaitForSeconds(1f);
        slide = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
