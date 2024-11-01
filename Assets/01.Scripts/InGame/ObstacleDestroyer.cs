using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] string colliObj;

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
            if (colliObj == "HitBox")
            {
                //BatWeapon Rank에 따라 점수 추가
                GameManager.Instance.score.AddScore(batRank);

                if (GameManager.Instance.playerController.eCh == ECharacter.Bat)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }

            }
            else if (colliObj == "Ball")
            {
                //GloveWeapon Rank에 따라 점수 추가
                GameManager.Instance.score.AddScore(gloveRank);
                if (GameManager.Instance.playerController.eCh == ECharacter.Glove)
                {
                    GameManager.Instance.weapon.GageUpdate();
                }
            }
            Destroy(gameObject);

        }
    }
}
