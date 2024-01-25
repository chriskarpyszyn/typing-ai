using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    [Header("Main Game Scores")]
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textTimer;

    [Header("End Game Scores")]
    //main game score canvas
    public TextMeshProUGUI textElapsedTime;
    public TextMeshProUGUI textTotalScore;
    public TextMeshProUGUI textStreak;
    public TextMeshProUGUI textFailures;

    private int score = 0;

    //end game stats
    private int keystrokeStreak = 0;
    private int keystrokeStreakMax = 0;
    private int failures = 0;
    private float elapsedTime = 0f;

    [SerializeField]
    private TMP_InputField inputHighScoreName;
    public UnityEvent<string, int> submitScoreEvent;

    /**
     * Submit score to the leaderboard
     */
    public void SubmitScore()
    {
        Debug.Log("SubmitScore --> Username: " + inputHighScoreName.text + " Score: " + int.Parse(textScore.text));
        submitScoreEvent.Invoke(inputHighScoreName.text, int.Parse(textScore.text)); //testing the string value of score
    }

    public int GetScore()
    {
        return this.score;
    }

    public string GetElapsedTime()
    {
        return this.elapsedTime.ToString("F1");
    }

    public int GetKeyStrokeMax()
    {
        return this.keystrokeStreakMax;
    }

    public int GetFailures()
    {
        return this.failures;
    }

    public void IncreaseScore(int v)
    {
        Debug.Log("Set Score " + v);
        this.score = this.score + v;
        textScore.text = this.score.ToString();
    }

    private void ResetScore()
    {
        this.score = 0;
    }

    public void ResetStats()
    {
        ResetScore();
        ResetElapsedTime();
        keystrokeStreakMax = 0;
        keystrokeStreak = 0;
        failures = 0;
        elapsedTime = 0;
    }

    public void IncrementFailures()
    {
        this.failures++;
    }

    public void IncrementKeystrokeStreak()
    {
        this.keystrokeStreak++;
        if (keystrokeStreak > keystrokeStreakMax)
        {
            keystrokeStreakMax = keystrokeStreak;
        }
    }

    public void ResetKeystrokeStreak()
    {
        this.keystrokeStreak = 0;
    }

    public void IncreaseElapsedTime(float t)
    {
        this.elapsedTime += t;
        textTimer.text = this.elapsedTime.ToString("F1");
    }

    private void ResetElapsedTime()
    {
        this.elapsedTime = 0;
    }

    public void DisplayEndGameStats()
    {
        textElapsedTime.text = "Time: " + this.elapsedTime.ToString("F1") + " seconds";
        textTotalScore.text = "Score: " + this.score;
        textStreak.text = "Longest Keystroke Streak: " + this.keystrokeStreakMax;
        textFailures.text = "Total Missed Keys: " + this.failures;
    }


}
