using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageToCoin : ActiveSkillBase
{
    public GameObject coin;

    public override void OnSkillStart()
    {
        //throw new System.NotImplementedException();
        Debug.Log("ChageToCoin Start");
    }

    public override void OnSkillUpdate()
    {
        //throw new System.NotImplementedException();
        int targetLayer = LayerMask.NameToLayer("Obstacle");

        foreach (Collider target in GetObjectsInRange(targetLayer))
        {
            ObstacleChageToCoin(target.gameObject);
            //Destroy(target.gameObject);
        }
    }

    public void ObstacleChageToCoin(GameObject target)
    {
        StartCoroutine(ObstacleChageToCoin_Cor(target));
    }

    IEnumerator ObstacleChageToCoin_Cor(GameObject target)
    {
        GameObject Coin = Instantiate(coin);

        if (GameManager.Instance.mapManager != null)
            Coin.transform.SetParent(GameManager.Instance.mapManager.GetCurrentMapObj().transform);

        Vector3 spawnPosition = target.gameObject.transform.position;
        spawnPosition.y += 0.5f;
        Coin.transform.position = spawnPosition;
        target.gameObject.SetActive(false);

        ParticlePoolManager.instance.SpawnEffect("ItemObtainEffect2", spawnPosition);
        yield return new WaitForSeconds(5f / GameManager.Instance.gameSpeed);

        target.gameObject.SetActive(true);
        Destroy(Coin);
    }
}
