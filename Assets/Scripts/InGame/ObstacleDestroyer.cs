using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] string colliObj;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(colliObj))
        {
            GameManager.Instance.score.IncreasObsScore();
            if (colliObj == "HitBox")
            {
                //BatWeapon Rank에 따라 점수 추가

                if (GameManager.Instance.playerController.eCh == ECharacter.Bat)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }

            }
            else if (colliObj == "Ball")
            {
                //GloveWeapon Rank에 따라 점수 추가
                Debug.Log("쓔우우우웅ㅅ");
                if (GameManager.Instance.playerController.eCh == ECharacter.Glove)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }
            }
            Destroy(gameObject);

        }
    }
}
