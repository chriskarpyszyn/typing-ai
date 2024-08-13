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

    private String userName;
    private float highScore;

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
        LeaderboardCreator.GetPersonalEntry(publicLeaderboardKey, ((msg) =>
        {
            this.userName = msg.Username;
            this.highScore = (float)msg.Score / 10;

            CanvasManager cm = GameObject.FindObjectOfType<CanvasManager>();
            cm.UpdateFinalScore(this.highScore, this.userName);

        }));

        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i<loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = ((float)msg[i].Score / 10).ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string userName, string timeValue)
    {
        float floatTime = float.Parse(timeValue);
        float timeValuex10 = floatTime * 10;
        int intTime = (int)Math.Round(timeValuex10);
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, userName, intTime, ((msg) =>
        {
            //LeaderboardCreator.ResetPlayer  
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

    public String GetUserName()
    {
        return userName;
    }

    public float GetHighScore()
    {
        return highScore;
    }
}
