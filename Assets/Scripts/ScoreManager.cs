using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetScore(int v)
    {
        this.score = this.score + v;
        textScore.text = this.score.ToString();
    }

    public void ResetStats()
    {
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

    public void TickElapsedTime(float t)
    {
        this.elapsedTime += t;
        textTimer.text = this.elapsedTime.ToString("F1");
    }

    public void DisplayEndGameStats()
    {
        textElapsedTime.text = "Time: " + this.elapsedTime.ToString("F1") + " seconds";
        textTotalScore.text = "Score: " + this.score;
        textStreak.text = "Longest Keystroke Streak: " + this.keystrokeStreakMax;
        textFailures.text = "Total Missed Keys: " + this.failures;
    }


}
