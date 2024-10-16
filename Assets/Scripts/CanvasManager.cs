using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public TextMeshProUGUI textTotalTime;
    public TMP_InputField leaderboardInputfield;
    public GameManager gm;

    private ScoreManager scoreManager;

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

        //current score is less than the high score
        if (Score.Instance.GetElapsedTime() >= leaderboardHighScore)
        {
            textTotalTime.text = "Your time: " + Score.Instance.GetElapsedTimeString();
        } else
        {
            textTotalTime.text = "New Best Time: " + Score.Instance.GetElapsedTimeString();
        }

        //if a name exists, populate the LeaderboardInputfield Text
        if (leaderboardUserName != NO_USERNAME_TEXT)
        {
            leaderboardInputfield.text = leaderboardUserName;
        }

    }
}
