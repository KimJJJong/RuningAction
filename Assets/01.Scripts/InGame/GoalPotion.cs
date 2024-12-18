using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GoalPotion : MonoBehaviour
{
    private bool ctlLock = false;
    private bool fade = false;

    private Renderer[] childRenderers;

    public Keeper keeper;

    [SerializeField]
    private GameObject dangerIcon1;

    [SerializeField]
    private GameObject dangerIcon2;

    private GameManager.Lane[] blockLane = new GameManager.Lane[2];

    public float kickDistance;

    // Start is called before the first frame update
    void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        keeper.transform.localPosition = new Vector3(-kickDistance, 0, 0);

        SetKeeper();
    }

    // Update is called once per frame
    void Update() { }

    void SetKeeper()
    {
        //Keeper.transform.localPosition =
        keeper.SetKeeperPosition(GameManager.Lane.Center);

        GameManager.Lane[] randomLanes = new GameManager.Lane[]
        {
            GameManager.Lane.Left,
            GameManager.Lane.Right,
        };

        System.Random random = new System.Random();
        int randIdx = random.Next(randomLanes.Length);
        GameManager.Lane lane = randomLanes[randIdx];

        dangerIcon1.transform.position = new Vector3(
            transform.position.x - kickDistance,
            dangerIcon1.transform.position.y,
            GameManager.Instance.lanePositions[GameManager.Lane.Center]
        );

        dangerIcon2.transform.position = new Vector3(
            transform.position.x - kickDistance,
            dangerIcon2.transform.position.y,
            GameManager.Instance.lanePositions[lane]
        );

        blockLane[0] = GameManager.Lane.Center;
        blockLane[1] = lane;
    }

    void SetCollisionBox()
    {
        BoxCollider box_collider = this.AddComponent<BoxCollider>();

        box_collider.isTrigger = true;
        box_collider.size = new Vector3(1f, 1f, 6f);
        box_collider.center = new Vector3(0, 0.5f, 0);
    }

    void Shoot(GameManager.Lane playerLane)
    {
        Debug.Log("Shoot");

        bool successFlag = true;
        foreach (GameManager.Lane lane in blockLane)
        {
            if (lane == playerLane)
            {
                successFlag = false;
                break;
            }
        }

        float duration = successFlag
            ? 0.5f / GameManager.Instance.gameSpeed
            : 1f / GameManager.Instance.gameSpeed;

        keeper.TryShootBlock(playerLane, duration);

        float dist = 15;

        if (!successFlag)
            dist -= 1;

        ctlLock = true;
        keeper.OnBallCollision = () =>
        {
            if (successFlag)
            {
                HpController.Heal(
                    GameManager
                        .Instance.playerManager.GetPlayerControllerByLane(GameManager.Lane.Left)
                        .playerObj,
                    100f
                );
                HpController.Heal(
                    GameManager
                        .Instance.playerManager.GetPlayerControllerByLane(GameManager.Lane.Center)
                        .playerObj,
                    100f
                );
                HpController.Heal(
                    GameManager
                        .Instance.playerManager.GetPlayerControllerByLane(GameManager.Lane.Right)
                        .playerObj,
                    100f
                );
                //GameManager.Instance.playerManager.ball.
                GameManager.Instance.playerManager.ball.ShootSuccessAction(keeper.gameObject);
            }
            else
            {
                GameManager.Instance.playerManager.ball.ShootBlockAction();
            }
        };
        GameManager
            .Instance.playerManager.ball.Shoot(kickDistance, duration)
            .OnFinish(() =>
            {
                fade = true;
                DOTween
                    .To(
                        () => 1f,
                        t =>
                        {
                            foreach (Renderer renderer in childRenderers)
                            {
                                renderer.material.color = new Color(
                                    renderer.material.color.r,
                                    renderer.material.color.g,
                                    renderer.material.color.b,
                                    t
                                );
                            }
                        },
                        0f,
                        3f / GameManager.Instance.gameSpeed
                    )
                    .OnKill(() =>
                    {
                        DOTween
                            .To(() => 0f, t => { }, 1f, 5f / GameManager.Instance.gameSpeed)
                            .OnKill(() =>
                            {
                                SetKeeper();
                                foreach (Renderer renderer in childRenderers)
                                {
                                    renderer.material.color = new Color(
                                        renderer.material.color.r,
                                        renderer.material.color.g,
                                        renderer.material.color.b,
                                        1.0f
                                    );
                                }
                                fade = false;
                                ctlLock = false;
                            });
                    });
            });
    }

    IEnumerator ReActive_Cor()
    {
        yield return new WaitForSeconds(5f / GameManager.Instance.gameSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ctlLock)
            return;

        PlayerController playerCtl = GameManager.Instance.playerManager.GetCurrentController();

        if (playerCtl.playerObj == other.gameObject)
        {
            Shoot(playerCtl.lane);
        }
    }
}
