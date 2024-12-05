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
    private int currentPlayer = 1;
    private CameraManager camera_manager;

    [SerializeField]
    public float jumpSpeed;

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

        leftController.position = EPlayerPosition.Left;
        centerController.position = EPlayerPosition.Center;
        rightController.position = EPlayerPosition.Right;
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
            GameManager.Instance.playerManager.jumpSpeed / GameManager.Instance.gameSpeed,
            0.5f
        );

        float jumpForce = GameManager.Instance.playerManager.jumpHeight;

        for (int i = 0; i < controller.jumpCount; i++)
            jumpForce *= 0.6f;

        Debug.Log("jumpForce: " + jumpForce);
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
            (currentPlayer == 0) ? leftController
            : (currentPlayer == 1) ? centerController
            : rightController;

        PlayerController to = from;

        if (isLeft)
        {
            to =
                (currentPlayer == 1) ? leftController
                : currentPlayer == 2 ? centerController
                : from;

            if (currentPlayer == 2)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.RtoC, passSpeed);
            }
            else if (currentPlayer == 1)
            {
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
                soccerBall.GetComponent<Ball>().Pass(PassType.LtoC, passSpeed);
            }
            else if (currentPlayer == 1)
            {
                soccerBall.GetComponent<Ball>().Pass(PassType.CtoR, passSpeed);
            }
            SwitchPlayer(1);
        }

        if (from == to)
            return;
        ctlLock = true;
        OnPass.Invoke(from, to);
    }

    private void SwitchPlayer(int direction)
    {
        int newPlayerNum = currentPlayer + direction;
        if (newPlayerNum < 0 || newPlayerNum > 2)
            return;

        PlayerController oldPlayerCtl = GetCurrentController();
        PlayerController newPlayerCtl = GetPlayerControllerByNumber(newPlayerNum);

        currentPlayer = newPlayerNum;

        Transform cameraTrans = newPlayerCtl.playerPosition.front;

        camera_manager.MoveCamera(newPlayerNum);

        oldPlayerCtl.MoveBack();
        newPlayerCtl.MoveFront();
    }

    private PlayerController GetPlayerControllerByNumber(int playerNum)
    {
        return playerNum switch
        {
            0 => leftController,
            1 => centerController,
            2 => rightController,
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
        return (currentPlayer == 0) ? leftController.playerObj
            : (currentPlayer == 1) ? centerController.playerObj
            : rightController.playerObj;
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
