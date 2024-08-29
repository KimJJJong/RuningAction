using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MH
{
    public class UpgradeManageButton : MonoBehaviour
    {

        public GameObject checkedOutline;

        void Start()
        {

        }

        void Update()
        {
            if (transform.GetSiblingIndex() == 0)
            {
                checkedOutline.SetActive(true);
            }
            else
            {
                checkedOutline.SetActive(false);
            }
        }

        public void checkThis()
        {
            transform.SetAsFirstSibling();
        }
    }
}

