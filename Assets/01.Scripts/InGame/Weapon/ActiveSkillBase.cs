using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActiveSkillBase : MonoBehaviour
{
    public float repeatInterval;
    public float range;

    private bool isActivable = true;

    private float activeCoolTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //SetCollisionBox();
        this.enabled = false;
    }

    void OnEnable()
    {
        OnSkillStart();
        activeCoolTime = repeatInterval;
    }

    void OnDisable() { }

    // Update is called once per frame
    void Update()
    {
        if (repeatInterval <= activeCoolTime)
        {
            OnSkillUpdate();
            activeCoolTime = 0.0f;
        }

        activeCoolTime += Time.deltaTime;
    }

    public abstract void OnSkillStart();
    public abstract void OnSkillUpdate();

    public List<Collider> GetObjectsInRange(int layer)
    {
        Vector3 target = GameManager.Instance.playerManager.transform.position;

        Vector3 center = new Vector3(target.x - range / 2, target.y, target.z);
        Vector3 halfExtents = new Vector3(range, 6f, 6f);

        Collider[] colliders = Physics.OverlapBox(
            center,
            halfExtents,
            GameManager.Instance.playerManager.transform.rotation,
            1 << layer
        );

        List<Collider> objectsInRange = new List<Collider>(colliders);

        return objectsInRange;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Vector3 target = GameManager.Instance.playerManager.transform.position;

        Vector3 center = new Vector3(target.x - range / 2, target.y, target.z);
        Vector3 halfExtents = new Vector3(range, 6f, 6f);

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, halfExtents);
    }
}
