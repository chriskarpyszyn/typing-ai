using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    private int score = 0;

    //end game stats
    private int keystrokeStreak = 0;
    private int keystrokeStreakMax = 0;
    private int failures = 0;
    private float elapsedTime = 0f;

    private LevelManager levelManager;

    private bool timerIsOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }

        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
            levelManager.OnGameCompleted += HandleGameCompleted;
            levelManager.OnNewGame += HandleNewGame;
        }
    }

    private void OnDisable()
    {
        if (levelManager != null)
        {
            levelManager.OnGameCompleted -= HandleGameCompleted;
            levelManager.OnNewGame -= HandleNewGame;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && timerIsOn)
        {
            IncreaseElapsedTime(Time.deltaTime);
        }
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
        timerIsOn = true;
        keystrokeStreakMax = 0;
        keystrokeStreak = 0;
        failures = 0;
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
        Debug.Log("RESET ELAPSED TIME" + elapsedTime);
        this.elapsedTime = 0;
        Debug.Log("RESET ELAPSED TIME" + elapsedTime);
    }

    private void HandleGameCompleted()
    {

        timerIsOn = false;

        //TODO-CK-17 Refactor this out.
        GameObject textTotalScoreObject = GameObject.Find("TextTotalScore");
        if (textTotalScoreObject != null)
        {
            TextMeshProUGUI textTotalScore = textTotalScoreObject.GetComponent<TextMeshProUGUI>();
            textTotalScore.text = "Total Score: " + this.score;
        }
    }

    private void HandleNewGame()
    {
        Debug.Log("ENTER: Handle New Game");
        ResetStats();
    }
}
