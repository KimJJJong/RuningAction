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
    public bool isDmg;
    public bool isDmgItem;

    [Header("Ball")]
    [SerializeField] private GameObject soccerBall;
    [SerializeField] private float ballMoveSpeed = 5f;
    private Transform targetTrans;
    [SerializeField] private PathArray[] leftToCenterPaht;
    [SerializeField] private PathArray[] centerToRightPaht;
    [SerializeField] private PathArray[] rightToCenterPaht;
    [SerializeField] private PathArray[] centerToLeftPaht;
    [SerializeField] private Transform[] path1Waypoints;
    [SerializeField] private Transform[] path2Waypoints;
    [SerializeField] private Transform[] path3Waypoints;

    private Transform[] selectedPath;
    private int currentWaypointIndex = 0;

    [Header("Speed")]
    [SerializeField] private float maxSpeed = 22f;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float speedIncreaseInterval = 0.5f;
    [SerializeField] private float timeSinceLastIncrease = 0f;

    private int currentPlayer = 1; //0 = ï¿½ï¿½ï¿½ï¿½, 1 = ï¿½ß¾ï¿½, 2 = ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    private CameraFollowPlayer cameraFollowPlayer;

    float ballTime = 0f;

    public void Start()
    {
        cameraFollowPlayer = Camera.main.GetComponent<CameraFollowPlayer>();

        Init();
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

    void Update()
    {
        if (ballTime > 0f)
        {
            ballTime -= Time.deltaTime;
            soccerBall.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            soccerBall.GetComponent<TrailRenderer>().enabled = false;
        }

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
                ballTime = 1f;
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchPlayer(1);
                ballTime = 1f;
            }
            /*
            if (targetTrans != null)
            {
                soccerBall.transform.position = Vector3.Lerp(soccerBall.transform.position, targetTrans.position, ballMoveSpeed * Time.deltaTime);
            }
            */
            if (targetTrans != null && selectedPath != null && selectedPath.Length > 0)
            {
                MoveBallAlongPath();
            }
        }
    }

    private void MoveBallAlongPath()
    {
        if (currentWaypointIndex < selectedPath.Length)
        {
            Transform waypoint = selectedPath[currentWaypointIndex];
            soccerBall.transform.position = Vector3.MoveTowards(soccerBall.transform.position, waypoint.position, ballMoveSpeed * Time.deltaTime);

            // waypoint¿¡ µµ´ÞÇßÀ» ¶§ ´ÙÀ½ À§Ä¡·Î ÀÌµ¿
            if (Vector3.Distance(soccerBall.transform.position, waypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // ±ËÀûÀ» ¸ðµÎ µû¶ó°¬À¸¸é ÃÖÁ¾ ¸ñÇ¥(targetTrans)·Î ¹Ù·Î ÀÌµ¿
            soccerBall.transform.position = Vector3.MoveTowards(soccerBall.transform.position, targetTrans.position, ballMoveSpeed * Time.deltaTime);
        }
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

        StartCoroutine(SmoothSwitchPlayer(newPlayer));
    }

    private IEnumerator SmoothSwitchPlayer(int newPlayerNum)
    {
        int oldPlayerNum = currentPlayer;
        GameObject oldPlayer = GetCurrentPlayer();
        GameObject newPlayer = GetPlayerByNumber(newPlayerNum);

        Vector3 oldStartPos, oldEndPos;
        Vector3 newStartPos, newEndPos;

        GetCurrentController().collisions.canInteract = false;
        currentPlayer = newPlayerNum;
        GetCurrentController().collisions.canInteract = true;

        Transform cameraTrans = (currentPlayer == 0) ? leftPos[1] : (currentPlayer == 1) ? centerPos[1] : rightPos[1];
        cameraFollowPlayer.StartCameraMove(cameraTrans);//GetCurrentPlayer().transform);

        GameManager.Instance.SetPlayer(GetCurrentController());

        int random = Random.Range(0, 3);
        switch (oldPlayerNum)
        {
            case 0:
                selectedPath = leftToCenterPaht[random].path;
                break;
            case 1:
                selectedPath = currentPlayer switch
                {
                    0 => centerToLeftPaht[random].path,
                    2 => centerToRightPaht[random].path,
                    _ => null
                };
                break;
            case 2:
                selectedPath = rightToCenterPaht[random].path;
                break;
            default:
                Debug.LogError("Wrong Path");
                break;
        }
        currentWaypointIndex = 0;

        float elapsedTime = 0f;
        const float duration = 0.3f;

        while (elapsedTime < duration)
        {
            SetSwitchPositions(oldPlayerNum, out oldStartPos, out oldEndPos, true);
            SetSwitchPositions(newPlayerNum, out newStartPos, out newEndPos, false);

            oldPlayer.transform.position = Vector3.Lerp(oldStartPos, oldEndPos, elapsedTime / duration);
            newPlayer.transform.position = Vector3.Lerp(newStartPos, newEndPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            PositionSoccerBall(GetCurrentController().ballPos);
            yield return null;
        }
    }

    private GameObject GetPlayerByNumber(int playerNum)
    {
        return playerNum switch
        {
            0 => leftPlayer,
            1 => centerPlayer,
            2 => rightPlayer,
            _ => null
        };
    }

    private void SetSwitchPositions(int playerNum, out Vector3 startPos, out Vector3 endPos, bool isFront)
    {
        Transform[] positions = playerNum switch
        {
            0 => leftPos,
            1 => centerPos,
            2 => rightPos,
            _ => null
        };

        if (positions != null)
        {
            startPos = isFront ? positions[1].position : positions[0].position;
            endPos = isFront ? positions[0].position : positions[1].position;
        }
        else
        {
            startPos = endPos = Vector3.zero;
        }
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

[System.Serializable]
public class PathArray
{
    public Transform[] path;
}

