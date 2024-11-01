using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayController : MonoBehaviour
{
    [Header("Start Pos")]
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;

    [Header("Character")]
    [SerializeField] private GameObject centerPlayer;
    [SerializeField] private GameObject leftPlayer;
    [SerializeField] private GameObject rightPlayer;

    [Header("Controller")]
    [SerializeField] private PlayerController centerController;
    [SerializeField] private PlayerController leftController;
    [SerializeField] private PlayerController rightController;

    [Header("Ball")]
    [SerializeField] private GameObject soccerBall;

    [Header("Speed")]
    [SerializeField] private float maxSpeed = 22f;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float speedIncreaseInterval = 0.5f;
    [SerializeField] private float timeSinceLastIncrease = 0f;

    private int currentPlayer = 1; //0 = 왼쪽, 1 = 중앙, 2 = 오른쪽
    private CameraFollowPlayer cameraFollowPlayer;

    public float moveSpeed = 5f; // 이동 속도
    private Transform targetTrans; // 목표 위치

    public void Start()
    {
        cameraFollowPlayer = Camera.main.GetComponent<CameraFollowPlayer>();

        Init();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
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

            if (targetTrans != null)
            {
                soccerBall.transform.position = Vector3.Lerp(soccerBall.transform.position, targetTrans.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void Init()
    {
        leftPlayer.transform.position = new Vector3(leftPos.position.x, transform.position.y, transform.position.z);
        centerPlayer.transform.position = new Vector3(centerPos.position.x, transform.position.y, transform.position.z + 0.5f);
        rightPlayer.transform.position = new Vector3(rightPos.position.x, transform.position.y, transform.position.z);

        leftController.collisions.canInteract = false;
        centerController.collisions.canInteract = false;
        rightController.collisions.canInteract = false;

        PositionSoccerBall(GetCurrentController().ballPos);
    }

    public void SetRunningAnimation(bool isRun)
    {
        centerController.SetRunningAnimation(isRun);
        leftController.SetRunningAnimation(isRun);
        rightController.SetRunningAnimation(isRun);

        GetCurrentController().collisions.canInteract = true;
    }

    private void Running()
    {
        Vector3 newPosition = transform.position + Vector3.forward * runningSpeed * Time.deltaTime;
        transform.position = newPosition;

        timeSinceLastIncrease += Time.deltaTime;
        if (timeSinceLastIncrease >= speedIncreaseInterval)
        {
            runningSpeed += accelerationRate;
            runningSpeed = Mathf.Clamp(runningSpeed, 0, maxSpeed);
            timeSinceLastIncrease = 0f;
        }
    }

    private void Jump()
    {
        GetCurrentController().SetState(EState.Up);
    }

    private void Slide()
    {
        GetCurrentController().SetState(EState.Down);
    }

    private void SwitchPlayer(int direction)
    {
        int newPlayer = currentPlayer + direction;
        if (newPlayer < 0 || newPlayer > 2) return;

        Transform newPos = (newPlayer == 0) ? leftPos : (newPlayer == 1) ? centerPos : rightPos;
        Transform currentPos = (currentPlayer == 0) ? leftPos : (currentPlayer == 1) ? centerPos : rightPos;

        GetCurrentController().collisions.canInteract = false;
        GetCurrentPlayer().transform.position = new Vector3(currentPos.position.x, transform.position.y, transform.position.z - 0.5f);

        currentPlayer = newPlayer;
        GetCurrentController().collisions.canInteract = true;
        GetCurrentPlayer().transform.position = new Vector3(newPos.position.x, transform.position.y, transform.position.z + 0.5f);

        cameraFollowPlayer.StartCameraMove(GetCurrentPlayer().transform);
        GameManager.Instance.SetPlayer(GetCurrentController());

        PositionSoccerBall(GetCurrentController().ballPos);
    }

    private void PositionSoccerBall(Transform target)
    {
        targetTrans = target;
    }

    public GameObject GetCurrentPlayer()
    {
        return (currentPlayer == 0) ? leftPlayer : (currentPlayer == 1) ? centerPlayer : rightPlayer;
    }

    public PlayerController GetCurrentController()
    {
        return (currentPlayer == 0) ? leftController : (currentPlayer == 1) ? centerController : rightController;
    }

    public float GetSpeed()
    {
        return runningSpeed;
    }
}
