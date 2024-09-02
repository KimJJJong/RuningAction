using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] string obstacle;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacle))
        {
            GameManager.Instance.score.IncreasObsScore();
            if (GameManager.Instance.playerController.eWeapons == EWeapon.Bat && obstacle == "HitBox")
                GameManager.Instance.weapon.GageUpdate();
            if (GameManager.Instance.playerController.eWeapons == EWeapon.Ball && obstacle == "Ball")
                GameManager.Instance.weapon.GageUpdate();
            Destroy(gameObject);

        }
    }
}
