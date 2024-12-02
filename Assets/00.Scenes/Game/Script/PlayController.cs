using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DarkTonic.MasterAudio;

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

    public void Start()
    {
        camera_manager = Camera.main.GetComponent<CameraManager>();

        Init();

        SetBall();

        GameManager.Instance.playController = this;

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
        GetCurrentController().SetState(EState.Runing);
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

        //GameManager.Instance.SetPlayer(GetCurrentController());

        float elapsedTime = 0f;
        float duration = passSpeed;

        SetSwitchPositions(oldPlayerNum, out oldStartPos, out oldEndPos, true);
        SetSwitchPositions(newPlayerNum, out newStartPos, out newEndPos, false);

        DOTween.To(
            () => oldStartPos,
            pos =>
            {
                Vector3 oldPos = oldPlayer.transform.position;
                oldPos.x = pos.x;
                oldPos.z = pos.z;
                oldPlayer.transform.position = oldPos;
            },
            oldEndPos,
            passSpeed
        );

        DOTween.To(
            () => newStartPos,
            pos =>
            {
                Vector3 newPos = newPlayer.transform.position;
                newPos.x = pos.x;
                newPos.z = pos.z;
                newPlayer.transform.position = newPos;
            },
            newEndPos,
            passSpeed
        );

        yield return null;

        /* while (elapsedTime < duration)
        {
            SetSwitchPositions(oldPlayerNum, out oldStartPos, out oldEndPos, true);
            SetSwitchPositions(newPlayerNum, out newStartPos, out newEndPos, false);

            oldPlayer.transform.position = Vector3.Lerp(
                new Vector3(oldStartPos.x, oldPlayer.transform.position.y, oldStartPos.z),
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
        } */
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
