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

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();

        //current score is less than the high score
        if (scoreManager.GetElapsedTimeFloat() >= leaderboardHighScore)
        {
            textTotalTime.text = "Your time: " + scoreManager.GetElapsedTimeString();
        } else
        {
            textTotalTime.text = "New Best Time: " + scoreManager.GetElapsedTimeString();
        }

        //if a name exists, populate the LeaderboardInputfield Text
        if (leaderboardUserName != NO_USERNAME_TEXT)
        {
            leaderboardInputfield.text = leaderboardUserName;
        }

    }
}
