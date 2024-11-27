using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum PassType
{
    CtoR,
    CtoL,
    RtoC,
    LtoC,
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
        BallRotate();
    }

    public void Shoot() { }

    public void Pass(PassType passType)
    {
        Pass(passType, 0);
    }

    public void Pass(PassType passType, float duration)
    {
        DOTweenPath pass;
        if (passList.TryGetValue(passType, out pass))
        {
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
        }
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
            else if (tweenPath.id == "LtoC")
                passList.Add(PassType.LtoC, tweenPath);

            AddTweenListener(tweenPath);
        }
    }

    private void AddTweenListener(DOTweenPath path)
    {
        path.onStart.AddListener(() =>
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
