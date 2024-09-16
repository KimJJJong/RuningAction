using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] string obstacle;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacle))
        {
            GameManager.Instance.score.IncreasObsScore();
            if (GameManager.Instance.playerController.eChWeapons == EWeapon.Bat && obstacle == "HitBox")
            {
                Debug.Log("ºü»þ");
                 GameManager.Instance.weapon.GageUpdate();
            }
            if (GameManager.Instance.playerController.eChWeapons == EWeapon.Glove && obstacle == "Ball")
                GameManager.Instance.weapon.GageUpdate();
            Destroy(gameObject);

        }
    }
}
