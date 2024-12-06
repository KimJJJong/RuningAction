using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string id;
    public string name;
    public float coolTime;
    public float activeTime;
    public float activeDurationTime;
    private float timeSinceLastActive = 0.0f;

    public abstract void Active();

    public abstract void Passive();

    private void Start()
    {
        Passive();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

        timeSinceLastActive += Time.deltaTime;

        if (timeSinceLastActive >= coolTime)
        {
            Active();
            timeSinceLastActive = 0.0f;
        }

        coolTime++;
    }
}
