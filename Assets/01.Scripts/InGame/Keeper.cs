using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    public GameObject goalPostObj;
    public GameObject goalKeeperObj;
    public GameObject goalNetObj;
    public BallCollisionAction OnBallCollision;

    // Start is called before the first frame update
    void Start()
    {
        OnBallCollision = () =>
        {
            GameManager.Instance.playerManager.ball.ShootBlockAction();
        };

        SphereCollider sc =
            GameManager.Instance.playerManager.ball.ballObject.GetComponent<SphereCollider>();
        if (sc != null)
        {
            ClothSphereColliderPair[] ClothColliders = new ClothSphereColliderPair[1];

            ClothColliders[0] = new ClothSphereColliderPair(sc);
            goalNetObj.GetComponent<Cloth>().sphereColliders = ClothColliders;
        }
    }

    // Update is called once per frame
    void Update() { }

    public void SetKeeperPosition(GameManager.Lane lane)
    {
        goalKeeperObj.transform.position = new Vector3(
            goalKeeperObj.transform.position.x,
            goalKeeperObj.transform.position.y,
            GameManager.Instance.lanePositions[lane]
        );
    }

    public void TryShootBlock(GameManager.Lane lane, float duration)
    {
        switch (lane)
        {
            case GameManager.Lane.Left:
                {
                    goalKeeperObj.GetComponent<Animator>().Play("MoveLeft");
                }
                break;
            case GameManager.Lane.Center:
                {
                    goalKeeperObj.GetComponent<Animator>().Play("Catch");
                }
                break;
            case GameManager.Lane.Right:
                {
                    goalKeeperObj.GetComponent<Animator>().Play("MoveRight");
                }
                break;
        }

        goalKeeperObj
            .transform.DOMoveZ(GameManager.Instance.lanePositions[lane], duration)
            .SetEase(Ease.InOutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.playerManager.ball.ballObject == other.gameObject)
            OnBallCollision?.Invoke();
    }

    public delegate void BallCollisionAction();
}
