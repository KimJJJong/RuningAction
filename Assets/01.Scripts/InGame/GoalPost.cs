using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    private bool ctlLock = false;
    private bool fade = false;

    private Renderer[] childRenderers;

    [Header("Keeper")]
    public GameObject Keeper;

    [SerializeField]
    private GameObject dangerIcon1;

    [SerializeField]
    private GameObject dangerIcon2;

    private GameManager.Lane[] blockLane = new GameManager.Lane[2];

    // Start is called before the first frame update
    void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        SetKeeper();
    }

    // Update is called once per frame
    void Update() { }

    void SetKeeper()
    {
        Keeper.transform.position = new Vector3(
            Keeper.transform.position.x,
            Keeper.transform.position.y,
            GameManager.Instance.lanePositions[GameManager.Lane.Center]
        );

        GameManager.Lane[] randomLanes = new GameManager.Lane[]
        {
            GameManager.Lane.Left,
            GameManager.Lane.Right,
        };

        System.Random random = new System.Random();
        int randIdx = random.Next(randomLanes.Length);
        GameManager.Lane lane = randomLanes[randIdx];

        dangerIcon1.transform.position = new Vector3(
            dangerIcon1.transform.position.x,
            dangerIcon1.transform.position.y,
            GameManager.Instance.lanePositions[GameManager.Lane.Center]
        );

        dangerIcon2.transform.position = new Vector3(
            dangerIcon2.transform.position.x,
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

        bool flag = true;
        foreach (GameManager.Lane lane in blockLane)
        {
            if (lane == playerLane)
            {
                flag = false;
                break;
            }
        }

        if (flag)
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
        }

        float z = 0f;
        float duration = flag
            ? 0.5f / GameManager.Instance.gameSpeed
            : 1f / GameManager.Instance.gameSpeed;
        switch (playerLane)
        {
            case GameManager.Lane.Left:
                Keeper.GetComponent<Animator>().Play("MoveLeft");
                z = GameManager.Instance.lanePositions[GameManager.Lane.Left];
                Keeper.transform.DOMoveZ(z, duration).SetEase(Ease.InOutQuad);
                break;
            case GameManager.Lane.Center:
                Keeper.GetComponent<Animator>().Play("Catch");
                z = GameManager.Instance.lanePositions[GameManager.Lane.Center];
                Keeper.transform.DOMoveZ(z, duration).SetEase(Ease.InOutQuad);
                break;
            case GameManager.Lane.Right:
                Keeper.GetComponent<Animator>().Play("MoveRight");
                z = GameManager.Instance.lanePositions[GameManager.Lane.Right];
                Keeper.transform.DOMoveZ(z, duration).SetEase(Ease.InOutQuad);
                break;
        }

        ctlLock = true;
        GameManager
            .Instance.playerManager.ball.Shoot()
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
                        1f / GameManager.Instance.gameSpeed
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
