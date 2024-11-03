using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayController : MonoBehaviour
{
    [Header("Start Pos")]
    [SerializeField] private Transform[] centerPos;
    [SerializeField] private Transform[] leftPos;
    [SerializeField] private Transform[] rightPos;

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
    private Transform newPos; // 캐릭터의 목표 위치
    private bool isSwitching = false; // 현재 전환 중인지 여부 확인
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

            if (targetTrans != null && !isSwitching)
            {
                soccerBall.transform.position = Vector3.Lerp(soccerBall.transform.position, targetTrans.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void Init()
    {
        leftPlayer.transform.position = leftPos[0].position;
        centerPlayer.transform.position = centerPos[1].position;
        rightPlayer.transform.position = rightPos[0].position;

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
        if (isSwitching) return;

        int newPlayer = currentPlayer + direction;
        if (newPlayer < 0 || newPlayer > 2) return;

        StartCoroutine(SmoothSwitchPlayer(newPlayer));
    }

    private IEnumerator SmoothSwitchPlayer(int newPlayerNum)
    {
        isSwitching = true;

        int oldPlayerNum = currentPlayer;
        GameObject oldPlayer = GetCurrentPlayer();
        Vector3 startPos;
        Vector3 endPos;
        if (oldPlayerNum == 0)
        {
            startPos = leftPos[1].transform.position;
            endPos = leftPos[0].transform.position;
        }
        else if (oldPlayerNum == 1)
        {
            startPos = centerPos[1].transform.position;
            endPos = centerPos[0].transform.position;
        }
        else
        {
            startPos = rightPos[1].transform.position;
            endPos = rightPos[0].transform.position;
        }
        GetCurrentController().collisions.canInteract = false;

        currentPlayer = newPlayerNum;
        Vector3 newStartPos;
        Vector3 newEndPos;
        GameObject newPlayer = GetCurrentPlayer();
        
        if (currentPlayer == 0)
        {
            newStartPos = leftPos[0].transform.position;
            newEndPos = leftPos[1].transform.position;
        }
        else if (currentPlayer == 1)
        {
            newStartPos = centerPos[0].transform.position;
            newEndPos = centerPos[1].transform.position;
        }
        else
        {
            newStartPos = rightPos[0].transform.position;
            newEndPos = rightPos[1].transform.position;
        }
        GetCurrentController().collisions.canInteract = true;

        cameraFollowPlayer.StartCameraMove(newPlayer.transform);

        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            oldPlayer.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            newPlayer.transform.position = Vector3.Lerp(newStartPos, newEndPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        oldPlayer.transform.position = endPos;
        newPlayer.transform.position = newEndPos;
        isSwitching = false;

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
