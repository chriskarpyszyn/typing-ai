using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    private ConfigData configData;

    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

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

    public void SetLeaderboardEntry(string userName, int scoreValue)
    {
        Debug.Log("SetLeaderboardEntry --> Username: " + userName + " Score: " + scoreValue);
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, userName, scoreValue, ((msg) =>
        {
            LeaderboardCreator.ResetPlayer();
            GetLeaderboard();
        }));
    }
}
