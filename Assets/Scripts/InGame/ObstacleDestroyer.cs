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
                //BatWeapon Rank�� ���� ���� �߰�

                if (GameManager.Instance.playerController.eCh == ECharacter.Bat)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }

            }
            else if (colliObj == "Ball")
            {
                //GloveWeapon Rank�� ���� ���� �߰�
                Debug.Log("�o�������");
                if (GameManager.Instance.playerController.eCh == ECharacter.Glove)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }
            }
            Destroy(gameObject);

        }
    }
}
