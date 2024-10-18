using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string userID;
    public UserData userData;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        userData = Resources.Load<UserData>("Datas/UserData/UserData " + userID);
    }
}
