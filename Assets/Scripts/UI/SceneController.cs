using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MH
{
    public class SceneController : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public void LoadSceneCount(int value)
        {
            SceneManager.LoadScene(value);
        }

        public void GameExit()
        {
            Application.Quit();
        }
    }
}


