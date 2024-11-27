using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    [Header("[Start Pos]")]
    [SerializeField]
    private Transform[] leftPos;

    [SerializeField]
    private Transform[] centerPos;

    [SerializeField]
    private Transform[] rightPos;

    [Header("[Character]")]
    [SerializeField]
    private GameObject leftPlayer;

    [SerializeField]
    private GameObject centerPlayer;

    [SerializeField]
    private GameObject rightPlayer;

    [Header("[Controller]")]
    [SerializeField]
    private PlayerController leftController;

    [SerializeField]
    private PlayerController centerController;

    [SerializeField]
    private PlayerController rightController;

    [SerializeField]
    private int currentPlayer = 1;
    private CameraManager camera_manager;

    public class PassEvent : UnityEngine.Events.UnityEvent<PlayerController, PlayerController> { }

    public static PassEvent OnPass = new PassEvent();

    public bool isDmg;
    public bool isDmgItem;

    [Header("[Speed]")]
    [SerializeField]
    private float maxSpeed = 22f;

    [SerializeField]
    public float runningSpeed;

    [SerializeField]
    private float accelerationRate;

    [SerializeField]
    private float speedIncreaseInterval = 0.5f;

    [SerializeField]
    private float timeSinceLastIncrease = 0f;

    [Header("[Ball]")]
    [SerializeField]
    private GameObject soccerBall;

    [SerializeField]
    private float ballMoveSpeed = 5f;
    private Transform targetTrans;

    [SerializeField]
    private DOTweenPath CToRPath;

    [SerializeField]
    private DOTweenPath CToLPath;

    [SerializeField]
    private DOTweenPath LtoCPath;

    [SerializeField]
    private DOTweenPath RtoCPath;

    [SerializeField]
    private PathArray[] leftToCenterPath;

    [SerializeField]
    private PathArray[] centerToLeftPath;

    [SerializeField]
    private PathArray[] centerToRightPath;

    [SerializeField]
    private PathArray[] rightToCenterPath;
    private Transform[] selectedPath;
    private DOTweenPath selectedTweenPath;
    private int currentWaypointIndex = 0;

    public float passSpeed = 0.5f;
    private float ballTime = 0f;

    private bool isPass = false;

    private bool ctlLock = true;

    public void Start()
    {
        camera_manager = Camera.main.GetComponent<CameraManager>();

        Init();

        initTween();

        GameManager.Instance.SetPlayer(centerController);

        GameManager.OnGameStateChange.AddListener(
            (state) =>
            {
                switch (state)
                {
                    case GameState.Playing:
                        ctlLock = false;
                        SetRunningAnimation(true);
                        break;
                    case GameState.GameOver:
                        SetRunningAnimation(false);
                        break;
                }
            }
        );
    }

    private void Init()
    {
        //cameraFollowPlayer.StartCameraMove(centerPos[1]);

        leftPlayer.transform.position = leftPos[0].position;
        centerPlayer.transform.position = centerPos[1].position;
        rightPlayer.transform.position = rightPos[0].position;

        leftController = leftPlayer.GetComponent<PlayerController>();
        centerController = centerPlayer.GetComponent<PlayerController>();
        rightController = rightPlayer.GetComponent<PlayerController>();

        leftController.collisions.canInteract = false;
        centerController.collisions.canInteract = false;
        rightController.collisions.canInteract = false;

        leftController.position = EPlayerPosition.Left;
        centerController.position = EPlayerPosition.Center;
        rightController.position = EPlayerPosition.Right;

        PositionSoccerBall(GetCurrentController().ballPos);
    }

    private void initTween()
    {
        //var tweenPaths = soccerBall.GetComponents<DOTweenPath>();
        /* var tweenPaths = soccerBall.GetComponentsInChildren<DOTweenPath>();

        CToRPath = Array.Find(tweenPaths, x => x.id == "CtoR");
        CToLPath = Array.Find(tweenPaths, x => x.id == "CtoL");
        LtoCPath = Array.Find(tweenPaths, x => x.id == "LtoC");
        RtoCPath = Array.Find(tweenPaths, x => x.id == "RtoC"); */
        isPass = false;

        //addTweenListner(CToRPath);
        //addTweenListner(CToLPath);
        //addTweenListner(RtoCPath);

        var ball = soccerBall.GetComponent<Ball>();
        if (ball == null)
        {
            Debug.LogError("ball is null");
            return;
        }
        ball.onPassStart.AddListener(() =>
        {
            passStart();
        });
        ball.OnPassComplete.AddListener(() =>
        {
            passEnd();
        });
    }

    public void addTweenListner(DOTweenPath path)
    {
        path.onStart.AddListener(() =>
        {
            passStart();
        });
        path.onComplete.AddListener(() =>
        {
            passEnd();
        });
        path.onRewind.AddListener(() =>
        {
            passEnd();
        });
        path.onUpdate.AddListener(() => {
            //Debug.Log("path :" + path.transform.position);
            //soccerBall.transform.position = path.transform.position;
        });
    }

    public void passStart()
    {
        isPass = true;
        ctlLock = true;
        Debug.Log("passStart");
    }

    public void passEnd()
    {
        isPass = false;
        ctlLock = false;
        GetCurrentController().SetState(EState.Runing);
        Debug.Log("passEnd");
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
            //soccerBall.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            //soccerBall.GetComponent<TrailRenderer>().enabled = false;
        }

        if (GameManager.Instance.gameState == GameState.Playing)
        {
            Controll();

            if (targetTrans != null && selectedPath != null && selectedPath.Length > 0)
            {
                //MoveBallAlongPath();
            }
        }
    }

    private void Controll()
    {
        //Running();

        if (ctlLock)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Pass(true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Pass(false);
        }
    }

    private void MoveBallAlongPath()
    {
        if (currentWaypointIndex < selectedPath.Length)
        {
            Transform waypoint = selectedPath[currentWaypointIndex];
            soccerBall.transform.position = Vector3.MoveTowards(
                soccerBall.transform.position,
                waypoint.position,
                ballMoveSpeed * Time.deltaTime
            );
            RotateTowardsTarget(waypoint.position);

            if (Vector3.Distance(soccerBall.transform.position, waypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            PositionSoccerBall(GetCurrentController().ballPos);
            soccerBall.transform.position = Vector3.MoveTowards(
                soccerBall.transform.position,
                targetTrans.position,
                ballMoveSpeed * Time.deltaTime
            );
            soccerBall.transform.rotation = Quaternion.identity;
        }
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - soccerBall.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        soccerBall.transform.rotation = Quaternion.RotateTowards(
            soccerBall.transform.rotation,
            targetRotation,
            360 * Time.deltaTime
        );
    }

    private void Running()
    {
        Vector3 newPosition = transform.position + Vector3.left * runningSpeed * Time.deltaTime;
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

    private void Pass(bool isLeft)
    {
        GetCurrentController().SetState(EState.Pass);

        PlayerController from =
            (currentPlayer == 0) ? leftController
            : (currentPlayer == 1) ? centerController
            : rightController;

        PlayerController to = from;

        //CToRPath.GetTween().timeScale = CToRPath.duration / (passSpeed * Time.timeScale);
        //CToLPath.GetTween().timeScale = CToLPath.duration / (passSpeed * Time.timeScale);
        //RtoCPath.GetTween().timeScale = RtoCPath.duration / (passSpeed * Time.timeScale);

        if (isLeft)
        {
            to =
                (currentPlayer == 1) ? leftController
                : currentPlayer == 2 ? centerController
                : from;

            if (currentPlayer == 2)
            {
                //RtoCPath.DORestart();
                //CToRPath.DOPlayBackwards();
                soccerBall.GetComponent<Ball>().Pass(PassType.RtoC, passSpeed);
                //DOTween.PlayBackwards("CtoR");
            }
            else if (currentPlayer == 1)
            {
                //CToLPath.DORestart();
                soccerBall.GetComponent<Ball>().Pass(PassType.CtoL, passSpeed);
            }
            SwitchPlayer(-1);
        }
        else
        {
            to =
                (currentPlayer == 0) ? centerController
                : currentPlayer == 1 ? rightController
                : from;
            if (currentPlayer == 0)
            {
                //CToLPath.DOPlayBackwards();
                soccerBall.GetComponent<Ball>().Pass(PassType.LtoC, passSpeed);
            }
            else if (currentPlayer == 1)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.CtoR, passSpeed);
                //CToRPath.DORestart();
                //DOTween.Restart("CtoR");
            }
            SwitchPlayer(1);
        }

        ballTime = passSpeed;

        if (from == to)
            return;
        ctlLock = true;
        OnPass.Invoke(from, to);
    }

    private void SwitchPlayer(int direction)
    {
        int newPlayer = currentPlayer + direction;
        if (newPlayer < 0 || newPlayer > 2)
            return;

        StartCoroutine(SmoothSwitchPlayer(newPlayer));
    }

    private IEnumerator SmoothSwitchPlayer(int newPlayerNum)
    {
        int oldPlayerNum = currentPlayer;
        GameObject oldPlayer = GetCurrentPlayer();
        GameObject newPlayer = GetPlayerByNumber(newPlayerNum);

        Vector3 oldStartPos,
            oldEndPos;
        Vector3 newStartPos,
            newEndPos;

        GetCurrentController().collisions.canInteract = false;
        currentPlayer = newPlayerNum;
        GetCurrentController().collisions.canInteract = true;

        Transform cameraTrans =
            (currentPlayer == 0) ? leftPos[1]
            : (currentPlayer == 1) ? centerPos[1]
            : rightPos[1];
        camera_manager.MoveCamera(newPlayerNum);

        GameManager.Instance.SetPlayer(GetCurrentController());

        int random = 0; //Random.Range(0, 3);
        switch (oldPlayerNum)
        {
            case 0:
                selectedPath = leftToCenterPath[random].path;
                break;
            case 1:
                selectedPath = currentPlayer switch
                {
                    0 => centerToLeftPath[random].path,
                    2 => centerToRightPath[random].path,
                    _ => null,
                };
                break;
            case 2:
                selectedPath = rightToCenterPath[random].path;
                break;
            default:
                Debug.LogError("Wrong Path");
                break;
        }
        currentWaypointIndex = 0;

        float elapsedTime = 0f;
        float duration = passSpeed;

        while (elapsedTime < duration)
        {
            SetSwitchPositions(oldPlayerNum, out oldStartPos, out oldEndPos, true);
            SetSwitchPositions(newPlayerNum, out newStartPos, out newEndPos, false);

            oldPlayer.transform.position = Vector3.Lerp(
                oldStartPos,
                oldEndPos,
                elapsedTime / duration
            );
            newPlayer.transform.position = Vector3.Lerp(
                newStartPos,
                newEndPos,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;

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
            _ => null,
        };
    }

    private void SetSwitchPositions(
        int playerNum,
        out Vector3 startPos,
        out Vector3 endPos,
        bool isFront
    )
    {
        Transform[] positions = playerNum switch
        {
            0 => leftPos,
            1 => centerPos,
            2 => rightPos,
            _ => null,
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
        return (currentPlayer == 0) ? leftPlayer
            : (currentPlayer == 1) ? centerPlayer
            : rightPlayer;
    }

    public PlayerController GetCurrentController()
    {
        return (currentPlayer == 0) ? leftController
            : (currentPlayer == 1) ? centerController
            : rightController;
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
