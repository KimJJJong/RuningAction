using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    //public string id;
    //public string name;
    public float coolTime;
    public float activeTime;

    private float lastActiveTime = 0.0f;
    private float nextActiveTime = 0.0f;

    private WeaponState curState = WeaponState.Activable;

    List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    List<ActiveSkillBase> activeSkills = new List<ActiveSkillBase>();

    private bool isActivable = true;
    private bool isSkillActive = false;
    private bool isSkillInProgress = false;

    private void ActivateSkill()
    {
        foreach (var skill in activeSkills)
        {
            skill.enabled = true;
        }
    }

    private void DeactivateSkill()
    {
        foreach (var skill in activeSkills)
        {
            skill.enabled = false;
        }
    }

    private void Start()
    {
        passiveSkills = GetComponents<PassiveSkill>().ToList();
        activeSkills = GetComponents<ActiveSkillBase>().ToList();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

        if (Time.time >= nextActiveTime)
        {
            NextState();
        }
    }

    void NextState()
    {
        float nextAddTime = 0f;

        lastActiveTime = Time.time;

        if (curState == WeaponState.Activable)
        {
            nextAddTime = activeTime;
            curState = WeaponState.Activing;
            ActivateSkill();
        }
        else if (curState == WeaponState.Activing)
        {
            if (coolTime == 0)
            {
                curState = WeaponState.Activable;
                return;
            }
            DeactivateSkill();

            nextAddTime = coolTime;
            curState = WeaponState.CoolTime;
        }
        else if (curState == WeaponState.CoolTime)
        {
            nextAddTime = 0;
            curState = WeaponState.Activable;
        }

        nextActiveTime = lastActiveTime + nextAddTime;
    }

    IEnumerator SkillTimer_Cor()
    {
        isActivable = false;

        isSkillActive = true;

        ActivateSkill();
        yield return new WaitForSeconds(activeTime);
        DeactivateSkill();

        isSkillActive = false;

        yield return new WaitForSeconds(coolTime);
        isActivable = true;
    }

    public enum WeaponState
    {
        Activable,
        Activing,
        CoolTime,
    }
}
