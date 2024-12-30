using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPassiveSkill : PassiveSkillBase
{
    public enum type
    {
        hp = 0,
        hp_dc = 1,
    }

    [System.Serializable]
    public struct test
    {
        public type _t;
        public float value;
    }

    public List<test> aaa;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
