using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("[Start Pos]")]
    [SerializeField]
    private Transform[] leftPos;

    [SerializeField]
    private Transform[] centerPos;

    [SerializeField]
    private Transform[] rightPos;

    [Header("[Controller]")]
    [SerializeField]
    private PlayerController leftController;

    [SerializeField]
    private PlayerController centerController;

    [SerializeField]
    private PlayerController rightController;

    [SerializeField]
    private GameManager.Lane currentLane = GameManager.Lane.NotDecidedYet;
    private CameraManager camera_manager;

    [SerializeField]
    public float jumpDuration;

    [SerializeField]
    public float jumpHeight;

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

    [HideInInspector]
    public Ball ball
    {
        get { return soccerBall.GetComponent<Ball>(); }
    }

    [SerializeField]
    private float ballMoveSpeed = 5f;

    public float passSpeed = 0.5f;
    private bool ctlLock = true;

    public void Awake()
    {
        camera_manager = Camera.main.GetComponent<CameraManager>();
        Init();
        SetBall();
        GameManager.Instance.playerManager = this;
    }

    public void Start()
    {
        GameManager.OnGameStateChange.AddListener(
            (state) =>
            {
                switch (state)
                {
                    case GameState.Playing:
                        ctlLock = false;
                        SetRunningAnimation(true);
                        currentLane = GameManager.Lane.Center;
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
        ctlLock = false;

        //cameraFollowPlayer.StartCameraMove(centerPos[1]);

        leftController.playerObj.transform.position = leftController.playerPosition.back.position;
        centerController.playerObj.transform.position = centerController
            .playerPosition
            .front
            .position;
        rightController.playerObj.transform.position = rightController.playerPosition.back.position;

        leftController.lane = GameManager.Lane.Left;
        centerController.lane = GameManager.Lane.Center;
        rightController.lane = GameManager.Lane.Right;
    }

    private void SetBall()
    {
        var ball = soccerBall.GetComponent<Ball>();
        if (ball == null)
        {
            Debug.LogError("Ball script is null");
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

    public void passStart()
    {
        ctlLock = true;
    }

    public void passEnd()
    {
        ctlLock = false;
        //GetCurrentController().SetState(EState.Runing);
    }

    public void SetRunningAnimation(bool isRun)
    {
        centerController.SetRunningAnimation(isRun);
        leftController.SetRunningAnimation(isRun);
        rightController.SetRunningAnimation(isRun);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            Controll();
        }
    }

    private void Controll()
    {
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
            MasterAudio.PlaySound("pass_1");
            Pass(true);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MasterAudio.PlaySound("pass_1");
            Pass(false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetCurrentController()
                .Jump()
                ?.OnComplete(() =>
                {
                    Debug.Log("Z Move Complete");
                })
                ?.OnFinish(() =>
                {
                    Debug.Log("Z Move Finish");
                });
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            GetCurrentController()
                .MoveBack()
                .OnComplete(() =>
                {
                    Debug.Log("X Move Complete");
                })
                .OnFinish(() =>
                {
                    Debug.Log("X Move Finish");
                });
        }
    }

    private void Jump()
    {
        PlayerController controller = GetCurrentController();

        if (controller.jumpCount >= controller.maxJumpCount)
            return;

        float jumpSpd = Math.Max(
            GameManager.Instance.playerManager.jumpDuration / GameManager.Instance.gameSpeed,
            0.5f
        );

        float jumpForce = GameManager.Instance.playerManager.jumpHeight;

        for (int i = 0; i < controller.jumpCount; i++)
            jumpForce *= 0.6f;

        controller.Jump(jumpForce, jumpSpd);

        ball.Jump(jumpForce + 0.5f, jumpSpd);
    }

    private void Slide()
    {
        GetCurrentController().Slide();
    }

    private void Pass(bool isLeft)
    {
        GetCurrentController().SetState(EState.Pass);

        PlayerController from =
            (currentLane == GameManager.Lane.Left) ? leftController
            : (currentLane == GameManager.Lane.Center) ? centerController
            : (currentLane == GameManager.Lane.Right) ? rightController
            : null;

        if (from == null)
            return;

        PlayerController to = from;

        if (isLeft)
        {
            to =
                (currentLane == GameManager.Lane.Center) ? leftController
                : currentLane == GameManager.Lane.Right ? centerController
                : from;

            if (currentLane == GameManager.Lane.Right)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.RtoC, passSpeed);
                SwitchPlayer(GameManager.Lane.Center);
            }
            else if (currentLane == GameManager.Lane.Center)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.CtoL, passSpeed);
                SwitchPlayer(GameManager.Lane.Left);
            }
            //SwitchPlayer(-1);
        }
        else
        {
            to =
                (currentLane == GameManager.Lane.Left) ? centerController
                : currentLane == GameManager.Lane.Center ? rightController
                : from;
            if (currentLane == GameManager.Lane.Left)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.LtoC, passSpeed);
                SwitchPlayer(GameManager.Lane.Center);
            }
            else if (currentLane == GameManager.Lane.Center)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.CtoR, passSpeed);
                SwitchPlayer(GameManager.Lane.Right);
            }
            //SwitchPlayer(1);
        }

        if (from == to)
            return;
        ctlLock = true;
        OnPass.Invoke(from, to);
    }

    private void LeftPass()
    {
        GetCurrentController().SetState(EState.Pass);
        PlayerController from = GetPlayerControllerByLane(currentLane);
        PlayerController to;

        if (!GetLeftLaneControllerFromCurrentLane(out to))
            return;
    }

    bool GetLeftLaneControllerFromCurrentLane(out PlayerController leftController)
    {
        try
        {
            int laneValue = (int)currentLane;
            while (laneValue >= (int)GameManager.Lane.Left)
            {
                GameManager.Lane lane = (GameManager.Lane)laneValue;
                leftController = GetPlayerControllerByLane(lane);

                if (leftController == null)
                    return false;

                if (!leftController.isDisabled)
                    return true;

                laneValue--;
            }
        }
        catch (Exception)
        {
            leftController = null;
            return false;
        }
        return false;
    }

    bool GetRightLaneControllerFromCurrentLane(out PlayerController rightController)
    {
        try
        {
            while (true)
            {
                GameManager.Lane lane = (GameManager.Lane)((int)currentLane + 1);
                rightController = GetPlayerControllerByLane(lane);

                if (rightController == null)
                    return false;

                if (!rightController.isDisabled)
                    return true;
            }
        }
        catch (Exception e)
        {
            rightController = null;
            return false;
        }
    }

    PassType GetPassType(PlayerController from, PlayerController to)
    {
        //TODO: 딕셔너리로 변경 해야됌
        if (from == leftController)
        {
            if (to == centerController)
                return PassType.LtoC;
            else if (to == rightController)
                return PassType.LtoR;
        }
        else if (from == centerController)
        {
            if (to == leftController)
                return PassType.CtoL;
            else if (to == rightController)
                return PassType.CtoR;
        }
        else if (from == rightController)
        {
            if (to == leftController)
                return PassType.RtoL;
            else if (to == centerController)
                return PassType.RtoC;
        }

        return PassType.None;
    }

    private void RightPass() { }

    private void SwitchPlayer(GameManager.Lane lane)
    {
        PlayerController oldPlayerCtl = GetPlayerControllerByLane(currentLane);
        PlayerController newPlayerCtl = GetPlayerControllerByLane(lane);

        currentLane = lane;

        Transform cameraTrans = newPlayerCtl.playerPosition.front;

        camera_manager.MoveCamera((int)lane);

        oldPlayerCtl.MoveBack();
        newPlayerCtl.MoveFront();
    }

    private PlayerController GetPlayerControllerByLane(GameManager.Lane playerNum)
    {
        return playerNum switch
        {
            GameManager.Lane.Left => leftController,
            GameManager.Lane.Center => centerController,
            GameManager.Lane.Right => rightController,
            _ => null,
        };
    }

    private GameObject GetPlayerByNumber(int playerNum)
    {
        return playerNum switch
        {
            0 => leftController.playerObj,
            1 => centerController.playerObj,
            2 => rightController.playerObj,
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

    public GameObject GetCurrentPlayer()
    {
        return (currentLane == GameManager.Lane.Left) ? leftController.playerObj
            : (currentLane == GameManager.Lane.Center) ? centerController.playerObj
            : rightController.playerObj;
    }

    public PlayerController GetCurrentController()
    {
        return (currentLane == GameManager.Lane.Left) ? leftController
            : (currentLane == GameManager.Lane.Center) ? centerController
            : rightController;
    }

    public float GetSpeed()
    {
        return runningSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SoundTrigger"))
        {
            string sound_id = other.GetComponent<SoundTrigger>().getTriggerSoundID();

            switch (sound_id)
            {
                case "car_horn":
                    MasterAudio.PlaySound("car_horn");
                    break;
            }
        }
    }
}
