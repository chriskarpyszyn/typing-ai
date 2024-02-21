using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan;
using Dan.Main;
using Unity.VisualScripting;
using System;

public class Leaderboard : MonoBehaviour
{
    private ConfigData configData;

    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    [SerializeField]
    private TMP_InputField userNameInputField; 

    //todo-ck for prod- remove, add to config, and do not deploy keys
    private string publicLeaderboardKey;

    private void Start()
    {
        LoadConfig();
        publicLeaderboardKey = configData.publicKey;
        GetLeaderboard();
    }

    private void LoadConfig()
    {
        TextAsset configFile = Resources.Load<TextAsset>("config");
        if (configFile != null)
        {
            configData = JsonUtility.FromJson<ConfigData>(configFile.text);
        }
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i<loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string userName, string timeValue)
    {
        Debug.Log("SetLeaderboardEntry --> Username: " + userName + " Time: " + timeValue);

        float floatTime = float.Parse(timeValue);
        int intTime = (int)Math.Round(floatTime); //todo-ck need to submit a float.

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, userName, intTime, ((msg) =>
        {
            //todo-ck need to convert decimal to int, and then do math when displaying back to string
            //LeaderboardCreator.ResetPlayer();
            
            GetLeaderboard();
        }));
    }

    public void EnableLeaderboardInputField()
    {
        userNameInputField.interactable = true;
    }

    public void DisableLeaderboardInputField()
    {
        userNameInputField.interactable = false;
    }
}
