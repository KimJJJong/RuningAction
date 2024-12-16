using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum PassType
{
    None = -1,
    CtoR,
    CtoL,
    RtoC,
    RtoL,
    LtoC,
    LtoR,
}

public class Ball : MonoBehaviour
{
    public GameObject ballObject;
    public UnityEvent onPassStart;
    public UnityEvent OnPassComplete;
    public UnityEvent onPassing;
    private Dictionary<PassType, DOTweenPath> passList = new Dictionary<PassType, DOTweenPath>();
    public Vector3 ballOffset;
    public float ballRotationSpeed;
    bool shootFlag = false;

    MoveActionWorker actionWorker = new MoveActionWorker();

    // Start is called before the first frame update
    void Awake()
    {
        if (onPassStart == null)
            onPassStart = new UnityEvent();
        if (OnPassComplete == null)
            OnPassComplete = new UnityEvent();
        if (onPassing == null)
            onPassing = new UnityEvent();
    }

    void Start()
    {
        SetPassList();
        transform.position = ballOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            BallRotate();
        }
    }

    public void Jump(float force, float duration)
    {
        if (shootFlag)
            return;
        //Vector3 positionOffset = transform.position;
        Tweener up = DOTween
            .To(
                () => transform.position,
                position => transform.position = position,
                transform.position + Vector3.up * force,
                duration / 2f
            )
            .SetEase(Ease.OutQuad);

        Tweener down = DOTween
            .To(
                () =>
                {
                    return transform.position;
                },
                position => transform.position = position,
                ballOffset,
                duration / 2f
            )
            .SetEase(Ease.InQuad);

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(up, down)
            .Build();

        actionWorker.Clear().AddAction(action);
    }

    public MoveActionWorker.MoveActionListener Shoot()
    {
        Vector3 endPos = ballObject.transform.position + Vector3.left * 15;
        endPos.y += 2;

        Tweener shoot = DOTween.To(
            () => ballObject.transform.position,
            t => ballObject.transform.position = t,
            endPos,
            1f / GameManager.Instance.gameSpeed
        );

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(shoot)
            .AddStartCallBack(() =>
            {
                shootFlag = true;
            })
            .AddFinishCallBack(() =>
            {
                shootFlag = false;
                GameManager.Lane lane = GameManager
                    .Instance.playerManager.GetCurrentController()
                    .lane;
                ballObject.transform.position = new Vector3(
                    ballOffset.x,
                    ballOffset.y,
                    GameManager.Instance.lanePositions[lane]
                );
            })
            .Build();

        actionWorker.Clear().AddAction(action);

        return action.listener;
    }

    public void Pass(PassType passType)
    {
        Pass(passType, 0);
    }

    public void Pass(PassType passType, float duration)
    {
        if (shootFlag)
            return;

        DOTweenPath pass;

        if (!passList.TryGetValue(passType, out pass))
            return;

        if (duration > 0)
        {
            duration = Math.Max(0.2f, duration / GameManager.Instance.gameSpeed);
            pass.GetTween().timeScale = pass.duration / (duration * Time.timeScale);
        }

        switch (passType)
        {
            case PassType.CtoR:
                break;
            case PassType.CtoL:
                break;
            case PassType.RtoC:
                break;
            case PassType.LtoC:
                break;
        }

        pass.DORestart();

        Tweener down = DOTween
            .To(
                () =>
                {
                    return transform.position;
                },
                position => transform.position = position,
                ballOffset,
                duration * 0.7f
            )
            .SetEase(Ease.OutSine);

        MoveActionWorker.MoveAction action = MoveActionWorker
            .ActionBuilder()
            .SetTweener(down)
            .Build();

        actionWorker.Clear().AddAction(action);
    }

    private void SetPassList()
    {
        passList = new Dictionary<PassType, DOTweenPath>();
        foreach (DOTweenPath tweenPath in gameObject.GetComponentsInChildren<DOTweenPath>())
        {
            if (tweenPath.id == "CtoR")
                passList.Add(PassType.CtoR, tweenPath);
            else if (tweenPath.id == "CtoL")
                passList.Add(PassType.CtoL, tweenPath);
            else if (tweenPath.id == "RtoC")
                passList.Add(PassType.RtoC, tweenPath);
            else if (tweenPath.id == "RtoL")
                passList.Add(PassType.RtoL, tweenPath);
            else if (tweenPath.id == "LtoC")
                passList.Add(PassType.LtoC, tweenPath);
            else if (tweenPath.id == "LtoR")
                passList.Add(PassType.LtoR, tweenPath);

            AddTweenListener(tweenPath);
        }
    }

    private void AddTweenListener(DOTweenPath path)
    {
        path.onPlay.AddListener(() =>
        {
            onPassStart.Invoke();
        });
        path.onComplete.AddListener(() =>
        {
            OnPassComplete.Invoke();
        });
        path.onRewind.AddListener(() =>
        {
            OnPassComplete.Invoke();
        });
        path.onUpdate.AddListener(() =>
        {
            if (ballObject != null)
                ballObject.transform.position = path.transform.position;

            onPassing.Invoke();
        });
    }

    private void BallRotate()
    {
        if (ballObject == null)
            return;

        ballObject.transform.Rotate(new Vector3(0, 0, ballRotationSpeed) * Time.deltaTime);
    }
}
