using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public TextMeshProUGUI textTotalTime;
    public TMP_InputField leaderboardInputfield;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFinalScore(float leaderboardHighScore, string leaderboardUserName)
    {
        string NO_USERNAME_TEXT = "Unknown";
        Debug.Log("Highscore: " + gm.GetScoreManager().GetElapsedTimeString());

        //Leaderboard lb = lbm.GetComponent<Leaderboard>();

        Debug.Log("current score " + gm.GetScoreManager().GetElapsedTimeFloat());
        Debug.Log("high score " + leaderboardHighScore);
        Debug.Log("user name " + leaderboardUserName);
        //current score is less than the high score
        if (gm.GetScoreManager().GetElapsedTimeFloat() >= leaderboardHighScore)
        {
            textTotalTime.text = "Your time: " + gm.GetScoreManager().GetElapsedTimeString();
        } else
        {
            textTotalTime.text = "New Best Time: " + gm.GetScoreManager().GetElapsedTimeString();
        }

        //if a name exists, populate the LeaderboardInputfield Text
        if (leaderboardUserName != NO_USERNAME_TEXT)
        {
            leaderboardInputfield.text = leaderboardUserName;
        }

    }
}
