using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager
{
    // Singleton instance
    private static ScoreManager _instance;

    // Property to access the instance
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreManager();
            }
            return _instance;
        }
    }

    //[Header("Main Game Scores")]
    //public TextMeshProUGUI textScore;
    //public TextMeshProUGUI textTimer;

    //[Header("End Game Scores")]
    ////main game score canvas
    //public TextMeshProUGUI textElapsedTime;
    //public TextMeshProUGUI textTotalScore;
    //public TextMeshProUGUI textStreak;
    //public TextMeshProUGUI textFailures;

    private int score = 0;

    //end game stats
    private int keystrokeStreak = 0;
    private int keystrokeStreakMax = 0;
    private int failures = 0;
    private float elapsedTime = 0f;

 
    

    private ScoreManager()
    {
        // Initialize your score manager here (e.g., set initial score)
    }
    
    public int GetScore()
    {
        return this.score;
    }

    public string GetElapsedTimeString()
    {
        return this.elapsedTime.ToString("F1");
    }

    public float GetElapsedTimeFloat()
    {
        return this.elapsedTime;
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
        //Debug.Log("Set Score " + v);
        this.score = this.score + v;

        //TODO-CK-17 Refactor this out.
        GameObject textScoreObject = GameObject.Find("TextScore");
        if (textScoreObject != null)
        {
            TextMeshProUGUI textScore = textScoreObject.GetComponent<TextMeshProUGUI>();
            textScore.text = this.score.ToString();
        }
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

        //TODO-CK-17 Refactor this out.
        GameObject textTimerObject = GameObject.Find("TextTimer");
        if (textTimerObject != null)
        {
            TextMeshProUGUI textTimer = textTimerObject.GetComponent<TextMeshProUGUI>();
            textTimer.text = this.elapsedTime.ToString("F1");
        }
    }

    private void ResetElapsedTime()
    {
        this.elapsedTime = 0;
    }

    public void DisplayEndGameStats()
    {

        //TODO-CK-17 Refactor this out.
        GameObject textTotalScoreObject = GameObject.Find("TextTotalScore");
        if (textTotalScoreObject != null)
        {
            TextMeshProUGUI textTotalScore = textTotalScoreObject.GetComponent<TextMeshProUGUI>();
            textTotalScore.text = "Total Score: " + this.score;
        }




        //textElapsedTime.text = "Time: " + this.elapsedTime.ToString("F1") + " seconds";
        //textStreak.text = "Longest Keystroke Streak: " + this.keystrokeStreakMax;
        //textFailures.text = "Total Missed Keys: " + this.failures;
    }


}
