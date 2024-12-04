using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField]
    string colliObj;

    int batRank;
    int gloveRank;

    private void Start()
    {
        //batRank = (int)GameManager.Instance.playerController.eBatRank;
        //gloveRank = (int)GameManager.Instance.playerController.eGloveRank;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(colliObj))
        {
            PlayerController playerController =
                GameManager.Instance.playerManager.GetCurrentController();
            if (colliObj == "HitBox")
            {
                //BatWeapon Rank�� ���� ���� �߰�
                playerController.score.AddScore(batRank);

                if (playerController.eCh == ECharacter.Bat)
                {
                    playerController.weapon.GageUpdate();
                }
            }
            else if (colliObj == "Ball")
            {
                //GloveWeapon Rank�� ���� ���� �߰�
                playerController.score.AddScore(gloveRank);
                if (playerController.eCh == ECharacter.Glove)
                {
                    playerController.weapon.GageUpdate();
                }
            }
            Destroy(gameObject);
        }
    }
}
