using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerManager : MonoBehaviour
{
    public UserData userData;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private string baseUrl = "���Ӽ���";

    //�α��� ���� ��û
    public IEnumerator RequestLoginInfo(string platformUserId, System.Action<LoginInfo> onSuccess)
    {
        string url = baseUrl + platformUserId;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Platform-ID", platformUserId);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoginInfo loginInfo = JsonUtility.FromJson<LoginInfo>(request.downloadHandler.text);
            onSuccess?.Invoke(loginInfo);
        }
        else
        {
            Debug.LogError("Failed to fetch login info: " + request.error);
        }
    }

    //���� ���� ��û
    public IEnumerator RequestUserInfo(string userId, System.Action onSuccess)
    {
        string url = baseUrl + userId;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            UserInfo userInfo = JsonUtility.FromJson<UserInfo>(request.downloadHandler.text);
            //userData.SetUserInfo(userInfo);
            onSuccess?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to fetch user info: " + request.error);
        }
    }

    //�������� ����
    public IEnumerator RequestStageInfo(string userId, System.Action<StageInfo> onSuccess)
    {
        string url = baseUrl + userId;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            StageInfo stageInfo = JsonUtility.FromJson<StageInfo>(request.downloadHandler.text);
            onSuccess?.Invoke(stageInfo);
        }
        else
        {
            Debug.LogError("Failed to fetch stage info: " + request.error);
        }
    }

    //���� ���� ������Ʈ
    public IEnumerator UpdateUserInfo(UserInfo userInfo, System.Action onComplete)
    {
        string url = baseUrl + userInfo;
        string json = JsonUtility.ToJson(userInfo);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to update user info: " + request.error);
        }
    }
}

[System.Serializable]
public class LoginInfo
{
    public string platform_id;
    public int point_info;
    public List<Mission> mission_list;
    public string avatar_info; //�̹��� ��� �Ǵ� Base64�� ���޵ȴٰ� ����
}

[System.Serializable]
public class UserInfo
{
    public string id;
    public int level;
    public int exp;
    public int gold;
    public int crystal;
    public int high_score;
    public List<int> upg_list;
    public List<int> item_list;
    public List<int> skill_list;
    public List<int> equipped_list;
}

[System.Serializable]
public class StageInfo
{
    public List<bool> stageClearFlags;

    public List<StageStatus> StageClearFlags;
    public StageInfo()
    {
        StageClearFlags = new List<StageStatus>();
    }

    public void SetStageClearStatus(int stageId, bool isClear)
    {
        for (int i = 0; i < StageClearFlags.Count; i++)
        {
            if (StageClearFlags[i].Stage_Id == stageId)
            {
                StageClearFlags[i] = new StageStatus(stageId, isClear);
                return;
            }
        }

        StageClearFlags.Add(new StageStatus(stageId, isClear));
    }

    public bool GetStageClearStatus(int stageId)
    {
        foreach (var status in StageClearFlags)
        {
            if (status.Stage_Id == stageId)
                return status.IsCleared;
        }
        return false;
    }
}

[System.Serializable]
public struct StageStatus
{
    public int Stage_Id;
    public bool IsCleared;

    public StageStatus(int stageId, bool isCleared)
    {
        Stage_Id = stageId;
        IsCleared = isCleared;
    }
}

[System.Serializable]
public class Mission
{
    public int missionId;
    public string description;
    public bool isCompleted;
}