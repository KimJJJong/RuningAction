using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace MH
{
    public class RankUIManager : MonoBehaviour
    {
        [SerializeField]
        private List<UserData> userDatas = new List<UserData>();

        public GameObject rankContent;
        public GameObject rankList;

        [SerializeField]
        private RankContent userRankContent;

        public TMP_Text userRank;
        public TMP_Text userName;
        public TMP_Text userScore;

        void Start()
        {
            UserData[] userDatas = Resources.LoadAll<UserData>("Datas/UserData");
            List<UserData> sortedUserData = userDatas.OrderByDescending(userData => userData.userScore).ToList();

            foreach (var obj in sortedUserData)
            {
                var content = Instantiate(rankContent, rankList.transform);
                content.GetComponent<RankContent>().SetData(obj);
                if (obj.userID == 1)
                {
                    userRankContent = content.GetComponent<RankContent>();
                    content.GetComponent<RankContent>().SetUser();
                    userName.text = obj.userName;
                    userScore.text = obj.userScore.ToString();
                }
            }
        }

        void Update()
        {
            userRank.text = userRankContent.rankText.text;
        }

    }
}
