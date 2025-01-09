using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeTrigger : MonoBehaviour
{
    bool ctlLock = false;
    public Transform target;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (ctlLock)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            MapPrefab map = GetComponentInParent<MapPrefab>();

            float y =
                map.prefab_bounds.size.y - GameManager.Instance.playerManager.transform.position.y;

            /* Vector3 target = transform.position;
            target.y = map.prefab_bounds.size.y + 1;
            target.x = 0; */
            GameManager.Instance.playerManager.MapChangeWithJumpAnim(target.position);
        }
    }
}
