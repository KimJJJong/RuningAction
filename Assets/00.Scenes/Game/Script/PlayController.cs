using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private GameObject centerPlayer;
    [SerializeField] private GameObject leftPlayer;
    [SerializeField] private GameObject rightPlayer;

    public float runningSpeed;
    public float maxSpeed = 22f;
    public float accelerationRate;

    private PlayerController centerController, leftController, rightController;
    private int currentPlayer = 1; //0 = ¿ÞÂÊ, 1 = Áß¾Ó, 2 = ¿À¸¥ÂÊ
    private CameraFollowPlayer cameraFollowPlayer;


    void Start()
    {
        cameraFollowPlayer = Camera.main.GetComponent<CameraFollowPlayer>();

        centerController = centerPlayer.GetComponent<PlayerController>();
        leftController = leftPlayer.GetComponent<PlayerController>();
        rightController = rightPlayer.GetComponent<PlayerController>();

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
        }
    }

    private void Init()
    {
        leftPlayer.transform.position = new Vector3(leftPos.position.x, transform.position.y, transform.position.z);
        centerPlayer.transform.position = new Vector3(centerPos.position.x, transform.position.y, transform.position.z + 1);
        rightPlayer.transform.position = new Vector3(rightPos.position.x, transform.position.y, transform.position.z);

        leftController.collisions.canInteract = false;
        centerController.collisions.canInteract = false;
        rightController.collisions.canInteract = false;
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

        runningSpeed += accelerationRate * Time.deltaTime;
        runningSpeed = Mathf.Clamp(runningSpeed, 0, maxSpeed);
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
        GetCurrentPlayer().transform.position = new Vector3(currentPos.position.x, transform.position.y, transform.position.z);

        currentPlayer = newPlayer;
        GetCurrentController().collisions.canInteract = true;
        GetCurrentPlayer().transform.position = new Vector3(newPos.position.x, transform.position.y, transform.position.z + 1);

        cameraFollowPlayer.StartCameraMove(GetCurrentPlayer().transform);
    }

    public GameObject GetCurrentPlayer()
    {
        return (currentPlayer == 0) ? leftPlayer : (currentPlayer == 1) ? centerPlayer : rightPlayer;
    }

    private PlayerController GetCurrentController()
    {
        return (currentPlayer == 0) ? leftController : (currentPlayer == 1) ? centerController : rightController;
    }
}
