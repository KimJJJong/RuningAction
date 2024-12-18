using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<Action> recordedActions = new List<Action>();
    private float lastInputTime = 0f;

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

        leftController.rightController = centerController;
        centerController.leftController = leftController;
        centerController.rightController = rightController;
        rightController.leftController = centerController;
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
            if (!ctlLock)
            {
                if (recordedActions.Count > 0)
                {
                    recordedActions.Last()();
                    recordedActions.Clear();
                }
            }

            KeyInputEventHandler();
        }
    }

    void KeyActionTask(Action action)
    {
        action();
    }

    private void KeyInputEventHandler()
    {
        if (lastInputTime > Time.time - 0.1f)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Jump();
            recordedActions.Add(() => Jump());
            lastInputTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Slide();
            recordedActions.Add(() => Slide());
            lastInputTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            recordedActions.Add(() =>
            {
                MasterAudio.PlaySound("pass_1");
                LeftPass();
            });
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            recordedActions.Add(() =>
            {
                MasterAudio.PlaySound("pass_1");
                RightPass();
            });
            lastInputTime = Time.time;
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

    public void Pass(PlayerController from, PlayerController to)
    {
        PassType passType = GetPassType(from, to);
        if (passType == PassType.None)
            return;

        GetCurrentController().SetState(EState.Pass);
        soccerBall.GetComponent<Ball>().Pass(passType, passSpeed);
        SwitchPlayer(to.lane);
        OnPass.Invoke(from, to);
    }

    private void LeftPass()
    {
        PlayerController from = GetPlayerControllerByLane(currentLane);
        PlayerController to;

        to = from.GetLeftController();
        if (to == null)
            return;

        Pass(from, to);
    }

    private void RightPass()
    {
        PlayerController from = GetPlayerControllerByLane(currentLane);
        PlayerController to;

        to = from.GetRightController();
        if (to == null)
            return;

        Pass(from, to);
    }

    //public void Shoot() { }

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

    public PlayerController GetPlayerControllerByLane(GameManager.Lane playerNum)
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
