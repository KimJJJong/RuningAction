using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace MH
{
    public class MainUIManager : MonoBehaviour
    {
        public List<UserData> userDatas = new List<UserData>();

        public GameObject rankContent;
        public GameObject rankList;

        public TMP_Text userRank;
        public TMP_Text userName;
        public TMP_Text userScore;

        void Start()
        {
            /*
            foreach (var obj in Resources.FindObjectsOfTypeAll<UserData>())
            {
                userDatas.Add(obj);
                var rank = Instantiate(rankContent).GetComponent<RankContent>();
                rank.SetData(obj);
                rank.transform.parent = rankList.transform;
                rank.transform.localScale = new Vector3(1, 1, 1);

                if (obj.GetUserID() == 1)
                {
                    rank.SetUser();
                }

            }

            RankContent[] ranks = rankList.GetComponentsInChildren<RankContent>();
            foreach (var rank in ranks)
            {

            }
            */

            UserData[] userDatas = Resources.LoadAll<UserData>("Datas/UserData");
            List<UserData> sortedUserData = userDatas.OrderByDescending(userData => userData.userScore).ToList();

            foreach (var obj in sortedUserData)
            {
                var content = Instantiate(rankContent, rankList.transform);
                content.GetComponent<RankContent>().SetData(obj);
                if (obj.userID == 1)
                {
                    content.GetComponent<RankContent>().SetUser();
                    userRank.text = content.GetComponent<RankContent>().rankText.text;
                    userName.text = obj.userName;
                    userScore.text = obj.userScore.ToString();
                }
            }
        }
    }
}
